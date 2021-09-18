using BankRestApi.Data;
using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Services
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly IStatementsRepository _statementsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public TransactionServices(IAccountsRepository accountsRepository,
                                    IStatementsRepository statementsRepository,
                                    IUnitOfWork unitOfWork,
                                    IConfiguration config)
        {
            _accountsRepository = accountsRepository;
            _statementsRepository = statementsRepository;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<Account> Withdraw(string accountNumber, decimal amount)
        {
            _unitOfWork.BeginTransaction();
            var balance = await _accountsRepository.GetBalance(accountNumber);
            var withdrawalFee =  Decimal.Parse(_config["WithdrawalFee"], CultureInfo.InvariantCulture);

            if (balance == null || balance < amount + withdrawalFee)
            {
                _unitOfWork.Rollback();
                if (balance == null)
                    throw new InvalidOperationException("Inexistent account.");
                else
                    throw new InvalidOperationException("insufficient funds.");
            }

            var updatedBalance = (decimal)balance - (amount + withdrawalFee);

            await _accountsRepository.UpdateBalance(accountNumber, updatedBalance);
            await _statementsRepository.Save(accountNumber, DateTime.UtcNow, "Withdrawal", -amount, updatedBalance + withdrawalFee);
            await _statementsRepository.Save(accountNumber, DateTime.UtcNow, "Withdrawal fee", -withdrawalFee, updatedBalance);
            _unitOfWork.Commit();

            return new Account(accountNumber, updatedBalance);
        }

        public async Task<IEnumerable<StatementEntry>> GetStatement(string accountNumber)
        {
            var result = (await _statementsRepository.Get(accountNumber))?.OrderBy(s => s.Date);

            if (!result.Any())
                throw new InvalidOperationException("Inexistent account.");

            return result;
        }

        public async Task Deposit(string accountNumber, decimal amount)
        {
            _unitOfWork.BeginTransaction();
            var balance = await _accountsRepository.GetBalance(accountNumber);

            if (balance == null)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Inexistent account.");
            }

            var depositPercentageFee = Decimal.Parse(_config["DepositPercentageFee"], CultureInfo.InvariantCulture);
            var updatedBalance = (decimal)balance + amount - amount * depositPercentageFee;

            await _accountsRepository.UpdateBalance(accountNumber, updatedBalance);
            await _statementsRepository.Save(accountNumber, DateTime.UtcNow, "Deposit", amount, updatedBalance + amount * depositPercentageFee);
            await _statementsRepository.Save(accountNumber, DateTime.UtcNow, "Deposit fee", -amount * depositPercentageFee, updatedBalance);
            _unitOfWork.Commit();
        }

        public async Task Transfer(string fromAccount, string toAccount, decimal amount)
        {
            _unitOfWork.BeginTransaction();
            var sourceBalance = await _accountsRepository.GetBalance(fromAccount);
            var destinationBalance = await _accountsRepository.GetBalance(toAccount);
            var transferFee = Decimal.Parse(_config["TransferFee"], CultureInfo.InvariantCulture);

            if (sourceBalance == null || destinationBalance == null || sourceBalance < amount + transferFee)
            {
                _unitOfWork.Rollback();

                if(sourceBalance == null)
                    throw new InvalidOperationException("Source account inexistent.");
                if (toAccount == null)
                    throw new InvalidOperationException("Destination account inexistent.");
                else
                    throw new InvalidOperationException("insufficient funds.");
            }

            var sourceUpdatedBalance = (decimal)sourceBalance - amount - transferFee; 
            var destinationUpdatedBalance = (decimal)destinationBalance + amount;

            await _accountsRepository.UpdateBalance(fromAccount, sourceUpdatedBalance);
            await _accountsRepository.UpdateBalance(toAccount, destinationUpdatedBalance);
            await _statementsRepository.Save(fromAccount, DateTime.UtcNow, "Transfer (to account " + toAccount + ")", -amount, sourceUpdatedBalance + transferFee);
            await _statementsRepository.Save(fromAccount, DateTime.UtcNow, "Transfer fee", -transferFee, sourceUpdatedBalance);
            await _statementsRepository.Save(toAccount, DateTime.UtcNow, "Transfer (from account " + fromAccount + ")", amount, destinationUpdatedBalance);
            _unitOfWork.Commit();
        }
    }
}

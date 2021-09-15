using BankRestApi.Data;
using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public Account withdraw(string accountNumber, decimal amount)
        {
            _unitOfWork.BeginTransaction();
            var balance =_accountsRepository.getBalance(accountNumber);
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

            _accountsRepository.updateBalance(accountNumber, updatedBalance);
            _statementsRepository.save(accountNumber, DateTime.UtcNow, "Withdrawal", -amount, updatedBalance + withdrawalFee);
            _statementsRepository.save(accountNumber, DateTime.UtcNow, "Withdrawal fee", -withdrawalFee, updatedBalance);
            _unitOfWork.Commit();

            return new Account(accountNumber, updatedBalance);
        }

        public IEnumerable<StatementEntry> getStatement(string accountNumber)
        {
            var result = _statementsRepository.get(accountNumber)?.OrderBy(s => s.Date);

            if (!result.Any())
                throw new InvalidOperationException("Inexistent account.");

            return result;
        }

        public void deposit(string accountNumber, decimal amount)
        {
            _unitOfWork.BeginTransaction();
            var balance = _accountsRepository.getBalance(accountNumber);

            if (balance == null)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException("Inexistent account.");
            }

            var depositPercentageFee = Decimal.Parse(_config["DepositPercentageFee"], CultureInfo.InvariantCulture);
            var updatedBalance = (decimal)balance + amount - amount * depositPercentageFee;

            _accountsRepository.updateBalance(accountNumber, updatedBalance);
            _statementsRepository.save(accountNumber, DateTime.UtcNow, "Deposit", amount, updatedBalance + amount * depositPercentageFee);
            _statementsRepository.save(accountNumber, DateTime.UtcNow, "Deposit fee", -amount * depositPercentageFee, updatedBalance);
            _unitOfWork.Commit();
        }

        public void transfer(string fromAccount, string toAccount, decimal amount)
        {
            _unitOfWork.BeginTransaction();
            var sourceBalance = _accountsRepository.getBalance(fromAccount);
            var destinationBalance = _accountsRepository.getBalance(toAccount);
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

            _accountsRepository.updateBalance(fromAccount, sourceUpdatedBalance);
            _accountsRepository.updateBalance(toAccount, destinationUpdatedBalance);
            _statementsRepository.save(fromAccount, DateTime.UtcNow, "Transfer (to account " + toAccount + ")", -amount, sourceUpdatedBalance + transferFee);
            _statementsRepository.save(fromAccount, DateTime.UtcNow, "Transfer fee", -transferFee, sourceUpdatedBalance);
            _statementsRepository.save(toAccount, DateTime.UtcNow, "Transfer (from account " + fromAccount + ")", amount, destinationUpdatedBalance);
            _unitOfWork.Commit();
        }
    }
}

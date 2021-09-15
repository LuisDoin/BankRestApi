using BankRestApi.Data;
using BankRestApi.Data.Repositories;
using BankRestApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        public Account withdraw(string accountNumber, double amount)
        {
            _unitOfWork.BeginTransaction();
            var balance =_accountsRepository.getBalance(accountNumber);
            var withdrawalFee =  Double.Parse(_config["WithdrawalFee"]);

            if (balance == null || balance < amount + withdrawalFee)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException();
            }

            var updatedBalance = (double)balance - (amount + withdrawalFee);

            _accountsRepository.updateBalance(accountNumber, updatedBalance);
            _statementsRepository.save(accountNumber, DateTime.Now, "Withdrawal", -amount, updatedBalance + withdrawalFee);
            _statementsRepository.save(accountNumber, DateTime.Now, "Withdrawal fee", -withdrawalFee, updatedBalance);
            _unitOfWork.Commit();

            return new Account(accountNumber, updatedBalance);
        }

        public IEnumerable<StatementEntry> getStatement(string accountNumber)
        {
            return _statementsRepository.get(accountNumber)?.OrderBy(s => s.Date);
        }

        public void deposit(string accountNumber, double amount)
        {
            _unitOfWork.BeginTransaction();
            var balance = _accountsRepository.getBalance(accountNumber);

            if (balance == null)
            {
                _unitOfWork.Rollback();
                throw new InvalidOperationException();
            }

            var depositPercentageFee = Double.Parse(_config["DepositPercentageFee"]);
            var updatedBalance = (double)balance + amount - amount * depositPercentageFee;

            _accountsRepository.updateBalance(accountNumber, updatedBalance);
            _statementsRepository.save(accountNumber, DateTime.Now, "Deposit", amount, updatedBalance + amount * depositPercentageFee);
            _statementsRepository.save(accountNumber, DateTime.Now, "Deposit fee", amount * depositPercentageFee, updatedBalance);
            _unitOfWork.Commit();
        }
    }
}

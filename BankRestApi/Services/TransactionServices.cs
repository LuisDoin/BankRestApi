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
            var withdrawalFee = Int32.Parse(_config["WithdrawalFee"]);


            if (balance == null || balance < amount + withdrawalFee)
            {
                _unitOfWork.Rollback();
                return null;
            }

            var updatedBalance = (double)balance - (amount + withdrawalFee);

            _accountsRepository.updateBalance(accountNumber, updatedBalance);
            _statementsRepository.save(accountNumber, DateTime.Now, "Withdrawal", -amount, updatedBalance + withdrawalFee);
            _statementsRepository.save(accountNumber, DateTime.Now, "WithdrawalFee", -withdrawalFee, updatedBalance);
            _unitOfWork.Commit();

            return new Account(accountNumber, updatedBalance);
        }

        public IEnumerable<StatementEntry> getStatement(string accountNumber)
        {
            return _statementsRepository.get(accountNumber)?.OrderBy(s => s.Date);
        }
    }
}

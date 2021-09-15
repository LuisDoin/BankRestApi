using BankRestApi.Data;
using BankRestApi.Data.Repositories;
using BankRestApi.Models;
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

        public TransactionServices(IAccountsRepository accountsRepository,
                                    IStatementsRepository statementsRepository,
                                    IUnitOfWork unitOfWork)
        {
            _accountsRepository = accountsRepository;
            _statementsRepository = statementsRepository;
            _unitOfWork = unitOfWork;
        }

        public Account withdraw(string accountNumber, double amount)
        {
            _unitOfWork.BeginTransaction();
            var balance =_accountsRepository.getBalance(accountNumber);
            
            if(balance == null || balance < amount)
            {
                _unitOfWork.Rollback();
                return null;
            }

            var updatedBalance = (double)balance - amount;

            _accountsRepository.updateBalance(accountNumber, updatedBalance);
            _statementsRepository.save(accountNumber, DateTime.Now, "Withdraw", -amount, updatedBalance);
            _unitOfWork.Commit();

            return new Account(accountNumber, updatedBalance);
        }

        public IEnumerable<StatementEntry> getStatement(string accountNumber)
        {
            return _statementsRepository.get(accountNumber)?.OrderBy(s => s.Date);
        }
    }
}

using FakeBank.ResultPattern.Errors;
using FakeBank.ResultPattern.Exceptions;
using FluentResults;

namespace FakeBank.ResultPattern.Models
{
    public class BankAccount
    {
        public string AccountNumber { get; }
        private decimal _balance;
        private bool _isBlocked;
        private readonly object _balanceLock = new object();

        public BankAccount(string accountNumber, decimal initialBalance, bool isBlocked = false)
        {
            AccountNumber = accountNumber;
            _balance = initialBalance;
            _isBlocked = isBlocked;
        }

        public void CheckIfBlocked()
        {
            if (_isBlocked)
                throw new AccountBlockedException($"A conta {AccountNumber} está bloqueada.");
        }

        public Result Withdraw(decimal amount)
        {
            lock (_balanceLock)
            {
                if (amount <= 0)
                    return Result.Fail(DomainErrors.InvalidTransactionAmount);

                if (_balance < amount)
                    return Result.Fail(DomainErrors.InsufficientFunds);

                _balance -= amount;
                return Result.Ok();
            }
        }

        public Result Deposit(decimal amount)
        {
            lock (_balanceLock)
            {
                if (amount <= 0)
                    return Result.Fail(DomainErrors.InvalidTransactionAmount);

                _balance += amount;
                return Result.Ok();
            }
        }
    }
}

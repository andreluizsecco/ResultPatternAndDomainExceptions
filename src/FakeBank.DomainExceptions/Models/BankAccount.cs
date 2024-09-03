using FakeBank.DomainExceptions.Exceptions;

namespace FakeBank.DomainExceptions.Models
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

        public void Withdraw(decimal amount)
        {
            lock (_balanceLock)
            {
                if (amount <= 0)
                    throw new InvalidTransactionAmountException("O valor da transação deve ser maior que zero.");

                if (_balance < amount)
                    throw new InsufficientFundsException($"Saldo insuficiente na conta {AccountNumber}.");

                _balance -= amount;
            }
        }

        public void Deposit(decimal amount)
        {
            lock (_balanceLock)
            {
                if (amount <= 0)
                    throw new InvalidTransactionAmountException("O valor do depósito deve ser maior que zero.");

                _balance += amount;
            }
        }

        public decimal GetBalance()
        {
            lock (_balanceLock)
            {
                return _balance;
            }
        }
    }
}

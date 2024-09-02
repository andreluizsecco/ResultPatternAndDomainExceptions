using FakeBank.DomainExceptions.Exceptions;
using FakeBank.DomainExceptions.Models;

namespace FakeBank.DomainExceptions.Repositories
{
    public class BankAccountRepository
    {
        private readonly List<BankAccount> _accounts;

        public BankAccountRepository()
        {
            // Fake data
            _accounts = new List<BankAccount>
            {
                new BankAccount("123456", 1000),
                new BankAccount("654321", 1000),
                new BankAccount("112233", 1000, true),
            };
        }

        public BankAccount GetAccountByNumber(string accountNumber)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
            {
                throw new AccountNotFoundException($"Conta com o número {accountNumber} não encontrada.");
            }
            return account;
        }
    }
}

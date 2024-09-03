using FakeBank.ResultPattern.Errors;
using FakeBank.ResultPattern.Models;
using FluentResults;

namespace FakeBank.ResultPattern.Repositories
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

        public virtual Result<BankAccount> GetAccountByNumber(string accountNumber)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
                return Result.Fail(DomainErrors.AccountNotFound);
            
            return Result.Ok(account);
        }
    }
}

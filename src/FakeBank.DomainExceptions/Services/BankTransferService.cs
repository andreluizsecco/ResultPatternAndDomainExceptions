using FakeBank.DomainExceptions.Exceptions;
using FakeBank.DomainExceptions.Models;
using FakeBank.DomainExceptions.Repositories;

namespace FakeBank.DomainExceptions.Services
{
    public class BankTransferService
    {
        private readonly BankAccountRepository _bankAccountRepository;

        public BankTransferService() =>
            _bankAccountRepository = new BankAccountRepository();

        public bool TransferFunds(string sourceAccountNumber, string destinationAccountNumber, decimal amount)
        {
            try
            {
                var sourceAccount = _bankAccountRepository.GetAccountByNumber(sourceAccountNumber);
                var destinationAccount = _bankAccountRepository.GetAccountByNumber(destinationAccountNumber);

                // Verifica se as contas estão bloqueadas antes de prosseguir
                sourceAccount.CheckIfBlocked();
                destinationAccount.CheckIfBlocked();

                // Fluxo normal de saque e depósito
                sourceAccount.Withdraw(amount);
                destinationAccount.Deposit(amount);

                return true;
            }
            catch (AccountBlockedException ex)
            {
                // TODO: Logar o erro
                throw;
            }
            catch (InsufficientFundsException ex)
            {
                // TODO: Logar o erro
                // TODO: Rollback do saque
                return false;
            }
            catch (AccountNotFoundException ex)
            {
                // TODO: Logar o erro
                return false;
            }
            catch (InvalidTransactionAmountException ex)
            {
                // TODO: Logar o erro
                return false;
            }
            catch (Exception ex)
            {
                // TODO: Logar o erro
                throw;
            }
        }
    }
}

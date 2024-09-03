using FakeBank.DomainExceptions.Exceptions;
using FakeBank.DomainExceptions.Repositories;

namespace FakeBank.DomainExceptions.Services
{
    public class BankTransferService
    {
        private readonly BankAccountRepository _bankAccountRepository;

        public BankTransferService(BankAccountRepository bankAccountRepository) =>
            _bankAccountRepository = bankAccountRepository;

        public void TransferFunds(string sourceAccountNumber, string destinationAccountNumber, decimal amount)
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
                throw;
            }
            catch (AccountNotFoundException ex)
            {
                // TODO: Logar o erro
                throw;
            }
            catch (InvalidTransactionAmountException ex)
            {
                // TODO: Logar o erro
                throw;
            }
            catch (Exception ex)
            {
                // TODO: Logar o erro
                throw;
            }
        }
    }
}

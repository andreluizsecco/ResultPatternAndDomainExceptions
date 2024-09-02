using FakeBank.ResultPattern.Repositories;
using FluentResults;

namespace FakeBank.ResultPattern.Services
{
    public class BankTransferService
    {
        private readonly BankAccountRepository _bankAccountRepository;

        public BankTransferService() =>
            _bankAccountRepository = new BankAccountRepository();

        public Result TransferFunds(string sourceAccountNumber, string destinationAccountNumber, decimal amount)
        {
            var sourceAccountResult = _bankAccountRepository.GetAccountByNumber(sourceAccountNumber);
            var destinationAccountResult = _bankAccountRepository.GetAccountByNumber(destinationAccountNumber);

            // Verifica se as contas existem
            if (sourceAccountResult.IsFailed || destinationAccountResult.IsFailed)
                return Result.Fail(sourceAccountResult.Errors.Concat(destinationAccountResult.Errors));

            var sourceAccount = sourceAccountResult.Value;
            var destinationAccount = destinationAccountResult.Value;
            
            // Verifica se as contas estão bloqueadas antes de prosseguir
            // Nesse caso estamos usando uma exceção para interromper o fluxo
            sourceAccount.CheckIfBlocked();
            destinationAccount.CheckIfBlocked();

            // Tenta sacar o valor da conta de origem
            var withDrawalResult = sourceAccount.Withdraw(amount);
            if (withDrawalResult.IsFailed)
                return Result.Fail(withDrawalResult.Errors);

            // Tenta depositar o valor na conta de destino
            var depositResult = destinationAccount.Deposit(amount);
            if (depositResult.IsFailed)
            {
                // TODO: Rollback do saque
                return Result.Fail(depositResult.Errors);
            }

            return Result.Ok();
        }
    }
}

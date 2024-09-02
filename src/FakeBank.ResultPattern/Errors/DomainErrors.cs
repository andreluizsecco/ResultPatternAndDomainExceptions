using FluentResults;

namespace FakeBank.ResultPattern.Errors
{
    public static class DomainErrors
    {
        private const string ErrorCode = nameof(ErrorCode);

        public static readonly IError AccountBlocked = new Error("A conta está bloqueada.").WithMetadata(ErrorCode, 1);
        public static readonly IError InsufficientFunds = new Error("Saldo insuficiente.").WithMetadata(ErrorCode, 2);
        public static readonly IError AccountNotFound = new Error("Conta não encontrada.").WithMetadata(ErrorCode, 3);
        public static readonly IError InvalidTransactionAmount = new Error("Valor da transação inválido.").WithMetadata(ErrorCode, 4);
    }
}

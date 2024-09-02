namespace FakeBank.DomainExceptions.Exceptions
{
    public class InvalidTransactionAmountException : DomainException
    {
        public InvalidTransactionAmountException(string message) : base(message) { }
    }
}

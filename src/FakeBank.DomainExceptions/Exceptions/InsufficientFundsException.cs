namespace FakeBank.DomainExceptions.Exceptions
{
    public class InsufficientFundsException : DomainException
    {
        public InsufficientFundsException(string message) : base(message) { }
    }
}

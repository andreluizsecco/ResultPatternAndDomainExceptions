namespace FakeBank.DomainExceptions.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException() { }

        public AccountNotFoundException(string message) : base(message) { }
    }
}

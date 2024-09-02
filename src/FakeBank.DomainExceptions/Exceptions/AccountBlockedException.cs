namespace FakeBank.DomainExceptions.Exceptions
{
    public class AccountBlockedException : DomainException
    {
        public AccountBlockedException(string message) : base(message) { }
    }
}

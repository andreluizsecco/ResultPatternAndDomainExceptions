namespace FakeBank.ResultPattern.Exceptions
{
    public class AccountBlockedException : DomainException
    {
        public AccountBlockedException(string message) : base(message) { }
    }
}

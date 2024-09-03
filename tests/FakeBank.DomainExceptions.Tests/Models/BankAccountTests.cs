using FakeBank.DomainExceptions.Exceptions;
using FakeBank.DomainExceptions.Models;
using FluentAssertions;

namespace FakeBank.DomainExceptions.Tests.Models
{
    public class BankAccountTests
    {
        [Fact]
        public void CheckIfBlocked_ShouldThrowAccountBlockedException_WhenAccountIsBlocked()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100, true);

            // Act
            var action = new Action(() => account.CheckIfBlocked());

            // Assert
            action.Should().Throw<AccountBlockedException>()
                .WithMessage("A conta 1234567890 está bloqueada.");
        }

        [Fact]
        public void CheckIfBlocked_ShouldNotThrowAccountBlockedException_WhenAccountIsNotBlocked()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100, false);

            // Act
            var action = new Action(() => account.CheckIfBlocked());

            // Assert
            action.Should().NotThrow<AccountBlockedException>();
        }

        [Fact]
        public void Withdraw_ShouldThrowInvalidTransactionAmountException_WhenAmountIsZero()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            var action = new Action(() => account.Withdraw(0));

            // Assert
            action.Should().Throw<InvalidTransactionAmountException>()
                .WithMessage("O valor da transação deve ser maior que zero.");
        }

        [Fact]
        public void Withdraw_ShouldThrowInvalidTransactionAmountException_WhenAmountIsNegative()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            var action = new Action(() => account.Withdraw(-50));

            // Assert
            action.Should().Throw<InvalidTransactionAmountException>()
                .WithMessage("O valor da transação deve ser maior que zero.");
        }

        [Fact]
        public void Withdraw_ShouldThrowInsufficientFundsException_WhenBalanceIsLessThanAmount()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            var action = new Action(() => account.Withdraw(150));

            // Assert
            action.Should().Throw<InsufficientFundsException>()
                .WithMessage("Saldo insuficiente na conta 1234567890.");
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance_WhenAmountIsLessThanBalance()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            account.Withdraw(50);

            // Assert
            account.GetBalance().Should().Be(50);
        }

        [Fact]
        public void Deposit_ShouldThrowInvalidTransactionAmountException_WhenAmountIsZero()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            var action = new Action(() => account.Deposit(0));

            // Assert
            action.Should().Throw<InvalidTransactionAmountException>()
                .WithMessage("O valor do depósito deve ser maior que zero.");
        }

        [Fact]
        public void Deposit_ShouldThrowInvalidTransactionAmountException_WhenAmountIsNegative()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            var action = new Action(() => account.Deposit(-50));

            // Assert
            action.Should().Throw<InvalidTransactionAmountException>()
                .WithMessage("O valor do depósito deve ser maior que zero.");
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance_WhenAmountIsGreaterThanZero()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            account.Deposit(50);

            // Assert
            account.GetBalance().Should().Be(150);
        }

        [Fact]
        public void GetBalance_ShouldReturnCorrectBalance()
        {
            // Arrange
            var account = new BankAccount("1234567890", 100);

            // Act
            var balance = account.GetBalance();

            // Assert
            balance.Should().Be(100);
        }
    }
}

using FakeBank.ResultPattern.Errors;
using FakeBank.ResultPattern.Exceptions;
using FakeBank.ResultPattern.Models;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;

namespace FakeBank.ResultPattern.Tests.Models
{
    public class BankAccountTests
    {
        [Fact]
        public void CheckIfBlocked_WhenAccountIsBlocked_ThrowsAccountBlockedException()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var isBlocked = true;
            var bankAccount = new BankAccount(accountNumber, initialBalance, isBlocked);

            // Act & Assert
            bankAccount.Invoking(x => x.CheckIfBlocked())
                .Should().Throw<AccountBlockedException>()
                .WithMessage($"A conta {accountNumber} está bloqueada.");
        }

        [Fact]
        public void CheckIfBlocked_WhenAccountIsNotBlocked_DoesNotThrowException()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var isBlocked = false;
            var bankAccount = new BankAccount(accountNumber, initialBalance, isBlocked);

            // Act & Assert
            bankAccount.Invoking(x => x.CheckIfBlocked())
                .Should().NotThrow<AccountBlockedException>();
        }

        [Fact]
        public void Withdraw_WhenAmountIsZero_ReturnsInvalidTransactionAmountError()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Withdraw(0);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.InvalidTransactionAmount);
            });
        }

        [Fact]
        public void Withdraw_WhenAmountIsNegative_ReturnsInvalidTransactionAmountError()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Withdraw(-100);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.InvalidTransactionAmount);
            });
        }

        [Fact]
        public void Withdraw_WhenAmountIsGreaterThanBalance_ReturnsInsufficientFundsError()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Withdraw(2000);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.InsufficientFunds);
            });
        }

        [Fact]
        public void Withdraw_WhenAmountIsLessThanOrEqualToBalance_ReturnsSuccessResult()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Withdraw(500);

            // Assert
            result.Should().BeSuccess();
        }

        [Fact]
        public void Deposit_WhenAmountIsZero_ReturnsInvalidTransactionAmountError()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Deposit(0);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.InvalidTransactionAmount);
            });
        }

        [Fact]
        public void Deposit_WhenAmountIsNegative_ReturnsInvalidTransactionAmountError()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Deposit(-100);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.InvalidTransactionAmount);
            });
        }

        [Fact]
        public void Deposit_WhenAmountIsGreaterThanZero_ReturnsSuccessResult()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var result = bankAccount.Deposit(500);

            // Assert
            result.Should().BeSuccess();
        }

        [Fact]
        public void GetBalance_ReturnsCurrentBalance()
        {
            // Arrange
            var accountNumber = "123456789";
            var initialBalance = 1000m;
            var bankAccount = new BankAccount(accountNumber, initialBalance);

            // Act
            var balance = bankAccount.GetBalance();

            // Assert
            balance.Should().Be(initialBalance);
        }
    }
}

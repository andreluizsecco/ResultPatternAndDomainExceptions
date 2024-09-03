using FakeBank.DomainExceptions.Exceptions;
using FakeBank.DomainExceptions.Models;
using FakeBank.DomainExceptions.Repositories;
using FakeBank.DomainExceptions.Services;
using FluentAssertions;
using Moq;

namespace FakeBank.DomainExceptions.Tests.Services
{
    public class BankTransferServiceTests
    {
        [Fact]
        public void TransferFunds_WhenSourceAccountIsBlocked_ShouldThrowAccountBlockedException()
        {
            // Arrange
            var sourceAccountNumber = "123";
            var destinationAccountNumber = "456";
            var amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 500, true);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500);

            var bankAccountRepository = new Mock<BankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(sourceAccountNumber)).Returns(sourceAccount);
            bankAccountRepository.Setup(r => r.GetAccountByNumber(destinationAccountNumber)).Returns(destinationAccount);

            var bankTransferService = new BankTransferService(bankAccountRepository.Object);

            // Act
            var action = new Action(() => bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount));

            // Assert
            action.Should().Throw<AccountBlockedException>();
        }

        [Fact]
        public void TransferFunds_WhenDestinationAccountIsBlocked_ShouldThrowAccountBlockedException()
        {
            // Arrange
            var sourceAccountNumber = "123";
            var destinationAccountNumber = "456";
            var amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 500);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500, true);

            var bankAccountRepository = new Mock<BankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(sourceAccountNumber)).Returns(sourceAccount);
            bankAccountRepository.Setup(r => r.GetAccountByNumber(destinationAccountNumber)).Returns(destinationAccount);

            var bankTransferService = new BankTransferService(bankAccountRepository.Object);

            // Act
            var action = new Action(() => bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount));

            // Assert
            action.Should().Throw<AccountBlockedException>();
        }

        [Fact]
        public void TransferFunds_WhenSufficientFunds_ShouldWithdrawFromSourceAccountAndDepositToDestinationAccount()
        {
            // Arrange
            var sourceAccountNumber = "123";
            var destinationAccountNumber = "456";
            var amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 500);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500);

            var bankAccountRepository = new Mock<BankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(sourceAccountNumber)).Returns(sourceAccount);
            bankAccountRepository.Setup(r => r.GetAccountByNumber(destinationAccountNumber)).Returns(destinationAccount);

            var bankTransferService = new BankTransferService(bankAccountRepository.Object);

            // Act
            bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            sourceAccount.GetBalance().Should().Be(400);
            destinationAccount.GetBalance().Should().Be(600);
        }

        [Fact]
        public void TransferFunds_WhenInsufficientFunds_ShouldThrowInsufficientFundsException()
        {
            // Arrange
            var sourceAccountNumber = "123";
            var destinationAccountNumber = "456";
            var amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 50);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500);

            var bankAccountRepository = new Mock<BankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(sourceAccountNumber)).Returns(sourceAccount);
            bankAccountRepository.Setup(r => r.GetAccountByNumber(destinationAccountNumber)).Returns(destinationAccount);

            var bankTransferService = new BankTransferService(bankAccountRepository.Object);

            // Act & Assert
            Assert.Throws<InsufficientFundsException>(() =>
                bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount));
        }

        [Fact]
        public void TransferFunds_WhenSourceAccountNotFound_ShouldThrowAccountNotFoundException()
        {
            // Arrange
            var sourceAccountNumber = "123";
            var destinationAccountNumber = "456";
            var amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 50);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500);

            var bankAccountRepository = new Mock<BankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(sourceAccountNumber)).Throws<AccountNotFoundException>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(destinationAccountNumber)).Returns(destinationAccount);

            var bankTransferService = new BankTransferService(bankAccountRepository.Object);

            // Act & Assert
            Assert.Throws<AccountNotFoundException>(() =>
                bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount));
        }

        [Fact]
        public void TransferFunds_WhenInvalidTransactionAmount_ShouldThrowInvalidTransactionAmountException()
        {
            // Arrange
            var sourceAccountNumber = "123";
            var destinationAccountNumber = "456";
            var amount = -100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 500);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500);

            var bankAccountRepository = new Mock<BankAccountRepository>();
            bankAccountRepository.Setup(r => r.GetAccountByNumber(sourceAccountNumber)).Returns(sourceAccount);
            bankAccountRepository.Setup(r => r.GetAccountByNumber(destinationAccountNumber)).Returns(destinationAccount);

            var bankTransferService = new BankTransferService(bankAccountRepository.Object);

            // Act & Assert
            Assert.Throws<InvalidTransactionAmountException>(() =>
                bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount));
        }

    }
}

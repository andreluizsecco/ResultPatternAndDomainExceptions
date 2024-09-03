using FakeBank.ResultPattern.Errors;
using FakeBank.ResultPattern.Models;
using FakeBank.ResultPattern.Repositories;
using FakeBank.ResultPattern.Services;
using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using FluentResults;
using Moq;
using FakeBank.ResultPattern.Exceptions;
using System;

namespace FakeBank.ResultPattern.Tests.Services
{
    public class BankTransferServiceTests
    {
        private readonly Mock<BankAccountRepository> _bankAccountRepositoryMock;
        private readonly BankTransferService _bankTransferService;

        public BankTransferServiceTests()
        {
            _bankAccountRepositoryMock = new Mock<BankAccountRepository>();
            _bankTransferService = new BankTransferService(_bankAccountRepositoryMock.Object);
        }

        [Fact]
        public void TransferFunds_WhenSourceAccountDoesNotExist_ShouldReturnFailedResult()
        {
            // Arrange
            string sourceAccountNumber = "123";
            string destinationAccountNumber = "456";
            decimal amount = 100;

            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(sourceAccountNumber))
                .Returns(Result.Fail(DomainErrors.AccountNotFound));
            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(destinationAccountNumber))
                .Returns(Result.Ok(new BankAccount(sourceAccountNumber, 500)));

            // Act
            var result = _bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.AccountNotFound);
            });
        }

        [Fact]
        public void TransferFunds_WhenDestinationAccountDoesNotExist_ShouldReturnFailedResult()
        {
            // Arrange
            string sourceAccountNumber = "123";
            string destinationAccountNumber = "456";
            decimal amount = 100;

            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(sourceAccountNumber))
                .Returns(Result.Ok(new BankAccount(sourceAccountNumber, 500)));
            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(destinationAccountNumber))
                .Returns(Result.Fail(DomainErrors.AccountNotFound));

            // Act
            var result = _bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.AccountNotFound);
            });
        }

        [Fact]
        public void TransferFunds_WhenSourceAccountIsBlocked_ShouldReturnFailedResult()
        {
            // Arrange
            string sourceAccountNumber = "123";
            string destinationAccountNumber = "456";
            decimal amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 500, true);

            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(sourceAccountNumber))
                .Returns(Result.Ok(sourceAccount));
            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(destinationAccountNumber))
                .Returns(Result.Ok(new BankAccount(destinationAccountNumber, 500)));

            // Act
            var action = () => _bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            action.Should().Throw<AccountBlockedException>()
                .WithMessage($"A conta {sourceAccountNumber} está bloqueada.");
        }

        [Fact]
        public void TransferFunds_WhenDestinationAccountIsBlocked_ShouldReturnFailedResult()
        {
            // Arrange
            string sourceAccountNumber = "123";
            string destinationAccountNumber = "456";
            decimal amount = 100;

            var destinationAccount = new BankAccount(destinationAccountNumber, 500, true);

            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(sourceAccountNumber))
                .Returns(Result.Ok(new BankAccount(sourceAccountNumber, 500)));
            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(destinationAccountNumber))
                .Returns(Result.Ok(destinationAccount));

            // Act
            var action = () => _bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            action.Should().Throw<AccountBlockedException>()
                .WithMessage($"A conta {destinationAccountNumber} está bloqueada.");
        }

        [Fact]
        public void TransferFunds_WhenWithdrawalFails_ShouldReturnFailedResult()
        {
            // Arrange
            string sourceAccountNumber = "123";
            string destinationAccountNumber = "456";
            decimal amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 50);

            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(sourceAccountNumber))
                .Returns(Result.Ok(sourceAccount));
            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(destinationAccountNumber))
                .Returns(Result.Ok(new BankAccount(destinationAccountNumber, 500)));

            // Act
            var result = _bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            result.Should().BeFailure().And.Satisfy(result =>
            {
                result.Errors.Should().ContainEquivalentOf(DomainErrors.InsufficientFunds);
            });
        }

        [Fact]
        public void TransferFunds_WhenTransferSucceeds_ShouldReturnOkResult()
        {
            // Arrange
            string sourceAccountNumber = "123";
            string destinationAccountNumber = "456";
            decimal amount = 100;

            var sourceAccount = new BankAccount(sourceAccountNumber, 500);
            var destinationAccount = new BankAccount(destinationAccountNumber, 500);

            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(sourceAccountNumber))
                .Returns(Result.Ok(sourceAccount));
            _bankAccountRepositoryMock.Setup(r => r.GetAccountByNumber(destinationAccountNumber))
                .Returns(Result.Ok(destinationAccount));

            // Act
            var result = _bankTransferService.TransferFunds(sourceAccountNumber, destinationAccountNumber, amount);

            // Assert
            result.Should().BeSuccess();
        }
    }
}

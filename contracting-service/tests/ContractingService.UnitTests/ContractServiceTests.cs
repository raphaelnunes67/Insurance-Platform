using Xunit;
using Moq;
using ContractingService.Application.Services;
using ContractingService.Domain.Interfaces;
using ContractingService.Application.DTOs;
using ContractingService.Domain.Entities;

namespace ContractingService.UnitTests;

public class ContractServiceTests
{
    private readonly Mock<IContractRepository> _contractRepositoryMock;
    private readonly ContractService _contractService;

    public ContractServiceTests()
    {
        _contractRepositoryMock = new Mock<IContractRepository>();
        _contractService = new ContractService(_contractRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateContract_Should_ReturnId_When_DataIsValid()
    {
        var dto = new CreateContractDto 
        { 
            CustomerName = "Samsung", 
            Value = 1000.00
        };

        _contractRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Contract>()))
                               .Returns(Task.CompletedTask);

        var result = await _contractService.CreateContractAsync(dto);

        Assert.NotEqual(Guid.Empty, result);
        
        _contractRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Contract>()), Times.Once);
    }

    [Fact]
    public async Task CreateContract_Should_ThrowException_When_ValueIsNegative()
    {
        var dto = new CreateContractDto 
        { 
            CustomerName = "Samsung", 
            Value = -150.00m 
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _contractService.CreateContractAsync(dto));
        
        Assert.Equal("The value must be positive.", exception.Message);

        _contractRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Contract>()), Times.Never);
    }
}
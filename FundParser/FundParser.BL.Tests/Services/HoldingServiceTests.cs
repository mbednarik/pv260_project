using AutoMapper;
using FundParser.BL.DTOs;
using FundParser.BL.Services.HoldingService;
using FundParser.Configuration;
using FundParser.DAL;
using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using FundParser.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class HoldingServiceTests
{
    private IHoldingService _holdingService;
    private UnitOfWork _unitOfWork;

    [SetUp]
    public void Setup()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<FundParserDbContext>()
            .UseInMemoryDatabase($"FundParser_test_db_{Guid.NewGuid()}")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        var dbContext = new FundParserDbContext(options);

        _unitOfWork = new UnitOfWork(
            dbContext,
            new Repository<Fund>(dbContext),
            new Repository<Company>(dbContext),
            new Repository<Holding>(dbContext),
            new Repository<HoldingDiff>(dbContext)
        );
        var mapper = new Mapper(new MapperConfiguration(MapperConfig.ConfigureMapper));

        _holdingService = new HoldingService(_unitOfWork, mapper);
    }

    [Test]
    public async Task GetHoldings_ValidData_ReturnsHoldings()
    {
        // Arrange
        await _unitOfWork.HoldingRepository.Insert(CreateHolding(1));
        await _unitOfWork.CommitAsync();

        // // Act
        var result = (await _holdingService.GetHoldings()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Shares, Is.EqualTo(1));
            Assert.That(result[0].MarketValue, Is.EqualTo(1));
            Assert.That(result[0].Weight, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task GetHoldings_NoData_ReturnsEmptyList()
    {
        // Act
        var result = (await _holdingService.GetHoldings()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task GetHoldings_MultipleHoldings_ReturnsHoldingsInCorrectOrder()
    {
        // Arrange
        await _unitOfWork.HoldingRepository.Insert(CreateHolding(1));
        await _unitOfWork.HoldingRepository.Insert(CreateHolding(2));
        await _unitOfWork.HoldingRepository.Insert(CreateHolding(3));
        await _unitOfWork.CommitAsync();

        // Act
        var result = (await _holdingService.GetHoldings()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(result[0].Weight, Is.LessThan(result[1].Weight));
        Assert.That(result[1].Weight, Is.LessThan(result[2].Weight));
    }

    [Test]
    public async Task AddHolding_ValidData_ReturnsHolding()
    {
        // Arrange
        var holding = new AddHoldingDTO
        {
            Fund = new AddFundDTO { Name = "fund1" },
            Company = new AddCompanyDTO { Cusip = "cusip1" },
            MarketValue = 1,
            Date = DateTime.Now,
            Weight = 1,
            Shares = 1
        };

        // Act
        var result = await _holdingService.AddHolding(holding);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Shares, Is.EqualTo(1));
            Assert.That(result.MarketValue, Is.EqualTo(1));
            Assert.That(result.Weight, Is.EqualTo(1));
        });
    }

    [Test]
    public void AddHolding_InvalidFundData_ThrowsException()
    {
        // Arrange
        var holding = new AddHoldingDTO
        {
            Fund = null, // Invalid data
            Company = new AddCompanyDTO { Cusip = "cusip1" },
            MarketValue = 1,
            Date = DateTime.Now,
            Weight = 1,
            Shares = 1
        };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _holdingService.AddHolding(holding));
    }

    [Test]
    public void AddHolding_InvalidCompanyData_ThrowsException()
    {
        // Arrange
        var holding = new AddHoldingDTO
        {
            Fund = new AddFundDTO { Name = "fund1" },
            Company = null, // Invalid data
            MarketValue = 1,
            Date = DateTime.Now,
            Weight = 1,
            Shares = 1
        };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _holdingService.AddHolding(holding));
    }

    [Test]
    public void AddHolding_NullHolding_ThrowsException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _holdingService.AddHolding(null));
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork.Dispose();
    }

    private static Holding CreateHolding(int var) => new Holding
    {
        Weight = var,
        MarketValue = var,
        Shares = var,
        Date = DateTime.Now,
        Company = new Company
        {
            Cusip = "cusip" + var,
            Name = "name" + var,
            Ticker = "ticker" + var,
        },
        Fund = new Fund
        {
            Name = "fund" + var
        }
    };
}
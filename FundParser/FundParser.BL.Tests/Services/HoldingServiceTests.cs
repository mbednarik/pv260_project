using AutoMapper;
using FundParser.BL.DTOs;
using FundParser.BL.Services.HoldingService;
using FundParser.Configuration;
using FundParser.DAL;
using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using FundParser.DAL.UnitOfWork;
using Moq;
using MockQueryable.Moq;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class HoldingServiceTests
{
    private IHoldingService _holdingService;
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<IRepository<Holding>> _holdingRepositoryMock;
    private Mock<IRepository<Company>> _companyRepositoryMock;
    private Mock<IRepository<Fund>> _fundRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _holdingRepositoryMock = new Mock<IRepository<Holding>>();
        _companyRepositoryMock = new Mock<IRepository<Company>>();
        _fundRepositoryMock = new Mock<IRepository<Fund>>();
        
        _companyRepositoryMock.Setup(m => m.GetQueryable())
            .Returns(new List<Company>
            {
                new Company()
                {
                    Name = "company",
                    Cusip = "cusip",
                    Ticker = "ticker"
                }
            }.BuildMock());

        _fundRepositoryMock.Setup(m => m.GetQueryable())
            .Returns(new List<Fund>
            {
                new Fund()
                {
                    Name = "fund"
                }
            }.BuildMock());

        _unitOfWorkMock = new Mock<UnitOfWork>(
            new Mock<FundParserDbContext>().Object,
            _fundRepositoryMock.Object,
            _companyRepositoryMock.Object,
            _holdingRepositoryMock.Object,
            new Mock<IRepository<HoldingDiff>>().Object
        );

        var mapper = new Mapper(new MapperConfiguration(MapperConfig.ConfigureMapper));
        _holdingService = new HoldingService(_unitOfWorkMock.Object, mapper);
    }

    [Test]
    public async Task GetHoldings_ValidData_ReturnsHoldings()
    {
        // Arrange
        _holdingRepositoryMock.Setup(m => m.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Holding> { CreateHolding(1) });

        // Act
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
        _holdingRepositoryMock.Setup(m => m.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Holding> { CreateHolding(1), CreateHolding(2), CreateHolding(3) });

        // Act
        var result = (await _holdingService.GetHoldings()).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Weight, Is.LessThan(result[1].Weight));
            Assert.That(result[1].Weight, Is.LessThan(result[2].Weight));
        });
    }

    [Test]
    public async Task AddHolding_ValidDataExistingCompanyAndFund_ReturnsHolding()
    {
        // Arrange
        _holdingRepositoryMock.Setup(m => m.Insert(It.IsAny<Holding>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateHolding(1));

        // Act
        var result = await _holdingService.AddHolding(new AddHoldingDTO
        {
            Fund = new AddFundDTO { Name = "fund" },
            Company = new AddCompanyDTO { Cusip = "cusip" },
            MarketValue = 1,
            Date = DateTime.Now,
            Weight = 1,
            Shares = 1
        });

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
    public async Task AddHolding_ValidDataNonExistingCompany_ReturnsHolding()
    {
        // Arrange
        _holdingRepositoryMock.Setup(m => m.Insert(It.IsAny<Holding>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateHolding(1));

        // Act
        var result = await _holdingService.AddHolding(new AddHoldingDTO
        {
            Fund = new AddFundDTO { Name = "fund" },
            Company = new AddCompanyDTO { Cusip = "nonExistingCusip" },
            MarketValue = 1,
            Date = DateTime.Now,
            Weight = 1,
            Shares = 1
        });

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
    public async Task AddHolding_ValidDataNonExistingFund_ReturnsHolding()
    {
        // Arrange
        _holdingRepositoryMock.Setup(m => m.Insert(It.IsAny<Holding>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateHolding(1));

        // Act
        var result = await _holdingService.AddHolding(new AddHoldingDTO
        {
            Fund = new AddFundDTO { Name = "nonExistingFund" },
            Company = new AddCompanyDTO { Cusip = "cusip" },
            MarketValue = 1,
            Date = DateTime.Now,
            Weight = 1,
            Shares = 1
        });

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
        Assert.That(() => _holdingService.AddHolding(holding), Throws.Exception);
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
        Assert.That(() =>_holdingService.AddHolding(holding), Throws.Exception);
    }
    
    [Test]
    public void AddHolding_NullHolding_ThrowsException()
    {
        // Arrange & Act & Assert
        Assert.That(() => _holdingService.AddHolding(null!), Throws.Exception);
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
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
    public class GetHoldingsTests : HoldingServiceTestsBase
    {
        [Test]
        public async Task GetHoldings_ValidData_ReturnsHoldings()
        {
            // Arrange
            var expectedHolding = CreateHolding(1);
            holdingRepositoryMock.Setup(m => m.GetAll(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Holding> { expectedHolding });

            // Act
            var result = (await holdingService.GetHoldings()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            AssertHolding(result.First(), expectedHolding);
        }

        [Test]
        public async Task GetHoldings_NoData_ReturnsEmptyList()
        {
            // Act
            var result = (await holdingService.GetHoldings()).ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(0));
        }
    }

    public class AddHoldingTests : HoldingServiceTestsBase
    {
        [SetUp]
        public new void Setup()
        {
            companyRepositoryMock.Setup(m => m.GetQueryable())
                .Returns(new List<Company>
                {
                    new Company()
                    {
                        Name = "company",
                        Cusip = "cusip",
                        Ticker = "ticker"
                    }
                }.BuildMock());

            fundRepositoryMock.Setup(m => m.GetQueryable())
                .Returns(new List<Fund>
                {
                    new Fund()
                    {
                        Name = "fund"
                    }
                }.BuildMock());
        }

        [Test]
        public void AddHolding_InvalidFundData_ThrowsException()
        {
            // Arrange
            var addHoldingDto = CreateAddHoldingDto(1);
            addHoldingDto.Fund = null!; // Invalid data
          
            // Act & Assert
            Assert.That(() => holdingService.AddHolding(addHoldingDto), Throws.Exception);
        }

        [Test]
        public void AddHolding_InvalidCompanyData_ThrowsException()
        {
            // Arrange
            var addHoldingDto = CreateAddHoldingDto(1);
            addHoldingDto.Company = null!; // Invalid data

            // Act & Assert
            Assert.That(() => holdingService.AddHolding(addHoldingDto), Throws.Exception);
        }

        [Test]
        public void AddHolding_NullHolding_ThrowsException()
        {
            // Arrange & Act & Assert
            Assert.That(() => holdingService.AddHolding(null!), Throws.Exception);
        }

        [Test]
        public async Task AddHolding_ValidDataExistingCompanyAndFund_ReturnsHolding()
        {
            // Arrange
            holdingRepositoryMock.Setup(m => m.Insert(It.IsAny<Holding>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateHolding(1));

            // Act
            var result = await holdingService.AddHolding(CreateAddHoldingDto(1));

            // Assert
            AssertHolding(result, CreateHolding(1));
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
            var expectedHolding = CreateHolding(1);
            holdingRepositoryMock.Setup(m => m.Insert(It.IsAny<Holding>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedHolding);

            // Act
            var result = await holdingService.AddHolding(CreateAddHoldingDto(1));

            // Assert
            AssertHolding(result, expectedHolding);
        }

        [Test]
        public async Task AddHolding_ValidDataNonExistingFund_ReturnsHolding()
        {
            // Arrange
            var expectedHolding = CreateHolding(1);
            holdingRepositoryMock.Setup(m => m.Insert(It.IsAny<Holding>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedHolding);

            // Act
            var addHoldingDto = CreateAddHoldingDto(1);
            addHoldingDto.Fund.Name = "nonExistingFund";
            var result = await holdingService.AddHolding(addHoldingDto);

            // Assert
            AssertHolding(result, expectedHolding);
        }

        private static AddHoldingDTO CreateAddHoldingDto(int var) => new AddHoldingDTO
        {
            Fund = new AddFundDTO { Name = "fund" },
            Company = new AddCompanyDTO { Cusip = "cusip" },
            MarketValue = var,
            Date = DateTime.Now,
            Weight = var,
            Shares = var
        };
    }

    public class HoldingServiceTestsBase
    {
        protected IHoldingService holdingService;
        protected Mock<UnitOfWork> unitOfWorkMock;
        protected Mock<IRepository<Holding>> holdingRepositoryMock;
        protected Mock<IRepository<Company>> companyRepositoryMock;
        protected Mock<IRepository<Fund>> fundRepositoryMock;

        [SetUp]
        public void Setup()
        {
            holdingRepositoryMock = new Mock<IRepository<Holding>>();
            companyRepositoryMock = new Mock<IRepository<Company>>();
            fundRepositoryMock = new Mock<IRepository<Fund>>();

            unitOfWorkMock = new Mock<UnitOfWork>(
                new Mock<FundParserDbContext>().Object,
                fundRepositoryMock.Object,
                companyRepositoryMock.Object,
                holdingRepositoryMock.Object,
                new Mock<IRepository<HoldingDiff>>().Object
            );

            var mapper = new Mapper(new MapperConfiguration(MapperConfig.ConfigureMapper));
            holdingService = new HoldingService(unitOfWorkMock.Object, mapper);
        }

        protected static Holding CreateHolding(int var) => new Holding
        {
            Id = var,
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

        protected static void AssertHolding(HoldingDTO? holding, Holding expectedHolding)
        {
            Assert.That(holding, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(holding.Shares, Is.EqualTo(expectedHolding.Shares), "Holding shares does not match.");
                Assert.That(holding.Weight, Is.EqualTo(expectedHolding.Weight), "Holding weight does not match.");
                Assert.That(holding.MarketValue, Is.EqualTo(expectedHolding.MarketValue),
                    "Holding marketValue does not match.");
                Assert.That(holding.Id, Is.EqualTo(expectedHolding.Id), "Holding id does not match.");
            });
        }
    }
}
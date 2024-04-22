using FundParser.BL.DTOs;
using FundParser.BL.Services.HoldingDiffCalculatorService;
using FundParser.BL.Services.HoldingDiffService;
using FundParser.DAL;
using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using FundParser.DAL.UnitOfWork;
using MockQueryable.Moq;
using Moq;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class HoldingDiffServiceTests
{
    private const int FundId = 1;
    private const int CompanyId = 1;

    private HoldingDiffService _holdingDiffService;
    private Mock<UnitOfWork> _unitOfWorkMock;
    private Mock<IRepository<HoldingDiff>> _holdingDiffRepositoryMock;
    private Mock<IRepository<Holding>> _holdingRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _holdingRepositoryMock = new Mock<IRepository<Holding>>();
        _holdingDiffRepositoryMock = new Mock<IRepository<HoldingDiff>>();

        _unitOfWorkMock = new Mock<UnitOfWork>(
            new Mock<FundParserDbContext>().Object,
            new Mock<IRepository<Fund>>().Object,
            new Mock<IRepository<Company>>().Object,
            _holdingRepositoryMock.Object,
            _holdingDiffRepositoryMock.Object
        );
        _holdingDiffService = new HoldingDiffService(
            _unitOfWorkMock.Object,
            new HoldingDiffCalculatorService()
        );
    }

    [Test]
    public async Task GetHoldingDiffs_CalculatedDiffNotExist_ReturnsCalculatedDiff()
    {
        // Arrange
        _holdingDiffRepositoryMock.Setup(m => m.GetQueryable())
            .Returns(new List<HoldingDiff>().BuildMock());

        var dateTime = DateTime.Now;
        var nextDateTime = dateTime.AddDays(1);

        _holdingRepositoryMock.Setup(m => m.GetQueryable())
            .Returns(
                new List<Holding>
                {
                    CreateHolding(1, dateTime), CreateHolding(3, dateTime.AddDays(2)),
                    CreateHolding(4, dateTime.AddDays(3)), CreateHolding(2, nextDateTime),
                }.BuildMock()
            );

        // Act
        var result = (await _holdingDiffService.GetHoldingDiffs(FundId, dateTime, nextDateTime)).ToList();

        // Assert
        _holdingDiffRepositoryMock.Verify(m => m.Insert(It.IsAny<HoldingDiff>(), It.IsAny<CancellationToken>()),
            Times.AtLeastOnce());
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        AssertHoldingDiff(result[0], CreateExpectedHoldingDiff(
            CreateHolding(1, dateTime),
            CreateHolding(2, nextDateTime)
        ));
    }

    [Test]
    public async Task GetHoldingDiffs_CalculatedDiffExist_ReturnsExistingDiff()
    {
        // Arrange
        var dateTime = DateTime.Now;
        var nextDateTime = dateTime.AddDays(1);
        var expected = CreateExpectedHoldingDiff(
            CreateHolding(1, dateTime),
            CreateHolding(2, nextDateTime)
        );
        var notExpected = CreateExpectedHoldingDiff(
            CreateHolding(3, nextDateTime.AddDays(2)),
            CreateHolding(4, nextDateTime.AddDays(4))
        );

        _holdingDiffRepositoryMock.Setup(m => m.GetQueryable())
            .Returns(new List<HoldingDiff> { expected, notExpected }.BuildMock());

        // Act
        var result = (await _holdingDiffService.GetHoldingDiffs(FundId, dateTime, nextDateTime)).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        AssertHoldingDiff(result[0], expected);
    }

    [Test]
    public async Task GetHoldingDiffs_MultipleCalculatedDiffsExist_ReturnsExistingDiffs()
    {
        // Arrange
        var dateTime = DateTime.Now;
        var nextDateTime = dateTime.AddDays(1);
        var expected = CreateExpectedHoldingDiff(
            CreateHolding(1, dateTime),
            CreateHolding(2, nextDateTime)
        );
        var expected2 = CreateExpectedHoldingDiff(
            CreateHolding(3, dateTime),
            CreateHolding(4, nextDateTime)
        );
        var notExpected = CreateExpectedHoldingDiff(
            CreateHolding(3, nextDateTime.AddDays(2)),
            CreateHolding(4, nextDateTime.AddDays(4))
        );

        _holdingDiffRepositoryMock.Setup(m => m.GetQueryable())
            .Returns(new List<HoldingDiff> { expected, expected2, notExpected }.BuildMock());

        // Act
        var result = (await _holdingDiffService.GetHoldingDiffs(FundId, dateTime, nextDateTime)).ToList();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            AssertHoldingDiff(result[0], expected);
            AssertHoldingDiff(result[1], expected2);
        });
    }

    private static void AssertHoldingDiff(HoldingDiffDTO? holdingDiffDto, HoldingDiff holdingDiff)
    {
        Assert.That(holdingDiffDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(holdingDiffDto.OldShares, Is.EqualTo(holdingDiff.OldShares));
            Assert.That(holdingDiffDto.SharesChange, Is.EqualTo(holdingDiff.SharesChange));
            Assert.That(holdingDiffDto.OldWeight, Is.EqualTo(holdingDiff.OldWeight));
            Assert.That(holdingDiffDto.WeightChange, Is.EqualTo(holdingDiff.WeightChange));
            Assert.That(holdingDiffDto.FundName, Is.EqualTo(holdingDiff.Fund.Name));
            Assert.That(holdingDiffDto.CompanyName, Is.EqualTo(holdingDiff.Company.Name));
            Assert.That(holdingDiffDto.Ticker, Is.EqualTo(holdingDiff.Company.Ticker));
        });
    }

    private static Holding CreateHolding(int var, DateTime date) => new Holding
    {
        Id = var,
        Weight = var,
        MarketValue = var,
        Shares = var,
        Date = date,
        CompanyId = CompanyId,
        Company = new Company
        {
            Id = CompanyId,
            Cusip = "cusip",
            Name = "name",
            Ticker = "ticker",
        },
        FundId = FundId,
        Fund = new Fund
        {
            Id = FundId,
            Name = "fund"
        }
    };

    private static HoldingDiff CreateExpectedHoldingDiff(Holding oldHolding, Holding newHolding) => new HoldingDiff
    {
        FundId = oldHolding.FundId,
        Fund = oldHolding.Fund,
        CompanyId = oldHolding.CompanyId,
        Company = oldHolding.Company,
        OldHoldingDate = oldHolding.Date,
        NewHoldingDate = newHolding.Date,
        OldHoldingId = oldHolding.Id,
        NewHoldingId = newHolding.Id,
        OldShares = oldHolding.Shares,
        SharesChange = newHolding.Shares - oldHolding.Shares,
        OldWeight = oldHolding.Weight,
        WeightChange = newHolding.Weight - oldHolding.Weight,
        OldHolding = oldHolding,
        NewHolding = newHolding
    };
}
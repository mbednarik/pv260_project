using FundParser.BL.Services.HoldingDiffCalculatorService;
using FundParser.DAL.Models;

namespace FundParser.BL.Tests.Services
{
    [TestFixture]
    public class HoldingDiffCalculatorServiceTests
    {
        private const int FundId = 1;

        private HoldingDiffCalculatorService _holdingDiffCalculator;

        [SetUp]
        public void SetUp()
        {
            _holdingDiffCalculator = new HoldingDiffCalculatorService();
        }

        [TestCaseSource(nameof(CalculateHoldingDiffsTestCases))]
        public void CalculateHoldingDiffs_TestCases(List<Holding> oldHoldings, List<Holding> newHoldings, List<HoldingDiff> expectedHoldingDiffs)
        {
            var result = _holdingDiffCalculator.CalculateHoldingDiffs(oldHoldings, newHoldings)
                .OrderBy(holdingDiff => holdingDiff.CompanyId)
                .ToList();

            expectedHoldingDiffs = expectedHoldingDiffs.OrderBy(holdingDiff => holdingDiff.CompanyId).ToList();

            Assert.That(result, Has.Count.EqualTo(expectedHoldingDiffs.Count));
            for (int i = 0; i < expectedHoldingDiffs.Count; i++)
            {
                AssertHoldingDiff(result[i], expectedHoldingDiffs[i]);
            }
        }

        private static IEnumerable<TestCaseData> CalculateHoldingDiffsTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new List<Holding>(),
                    new List<Holding>(),
                    new List<HoldingDiff>()).SetName("{m}_Empty_ReturnsNoHoldingDiffs");

                var newHoldingDiff1 = GetNewHoldingDiff(5, 10, 1, 1);
                var newHoldingDiff2 = GetNewHoldingDiff(8, 6, 2, 2);
                var newHoldingDiff3 = GetNewHoldingDiff(1, 5, 3, 3);
                yield return new TestCaseData(
                    new List<Holding>(),
                    new List<Holding>
                    {
                        newHoldingDiff1.Item1,
                        newHoldingDiff2.Item1,
                        newHoldingDiff3.Item1,
                    },
                    new List<HoldingDiff>
                    {
                        newHoldingDiff1.Item2,
                        newHoldingDiff2.Item2,
                        newHoldingDiff3.Item2,
                    }).SetName("{m}_NoOldHoldings_ReturnsNewHoldingDiffs");

                var oldHoldingDiff1 = GetOldHoldingDiff(12, 3, 1, 1);
                var oldHoldingDiff2 = GetOldHoldingDiff(9, 5, 2, 2);
                var oldHoldingDiff3 = GetOldHoldingDiff(1, 4, 3, 3);
                yield return new TestCaseData(
                    new List<Holding>
                    {
                        oldHoldingDiff1.Item1,
                        oldHoldingDiff2.Item1,
                        oldHoldingDiff3.Item1,
                    },
                    new List<Holding>(),
                    new List<HoldingDiff>
                    {
                        oldHoldingDiff1.Item2,
                        oldHoldingDiff2.Item2,
                        oldHoldingDiff3.Item2,
                    }).SetName("{m}_AllHoldingRemoved_ReturnsOldHoldingDiffs");

                var changedHoldingDiff1 = GetChangedHoldingDiff(5, 12, 2, 3, 1, 1, 6);
                var changedHoldingDiff2 = GetChangedHoldingDiff(3, 5, 9, 14, 2, 2, 5);
                var changedHoldingDiff3 = GetChangedHoldingDiff(40, 8, 4, 1, 3, 3, 4);
                var changedHoldingDiff4 = GetChangedHoldingDiff(10, 10, 5, 5, 4, 5, 5);
                yield return new TestCaseData(
                    new List<Holding>
                    {
                        changedHoldingDiff1.Item1,
                        changedHoldingDiff2.Item1,
                        changedHoldingDiff3.Item1,
                        changedHoldingDiff4.Item1,
                    },
                    new List<Holding>
                    {
                        changedHoldingDiff1.Item2,
                        changedHoldingDiff2.Item2,
                        changedHoldingDiff3.Item2,
                        changedHoldingDiff4.Item2,
                    },
                    new List<HoldingDiff>
                    {
                        changedHoldingDiff1.Item3,
                        changedHoldingDiff2.Item3,
                        changedHoldingDiff3.Item3,
                        changedHoldingDiff4.Item3,
                    }).SetName("{m}_ChangedHoldings_ReturnsChangedHoldingDiffs");

                var mixedNewHoldingDiff1 = GetNewHoldingDiff(1, 5, 1, 1);
                var mixedChangedHoldingDiff2 = GetChangedHoldingDiff(40, 8, 4, 1, 2, 2, 3);
                var mixedOldHoldingDiff3 = GetOldHoldingDiff(1, 4, 3, 4);
                var mixedChangedHoldingDiff4 = GetChangedHoldingDiff(3, 5, 9, 14, 4, 5, 6);
                var mixedNewHoldingDiff5 = GetNewHoldingDiff(8, 6, 5, 7);
                var mixedOldHoldingDiff6 = GetOldHoldingDiff(12, 3, 6, 8);
                yield return new TestCaseData(
                    new List<Holding>
                    {
                        mixedChangedHoldingDiff2.Item1,
                        mixedOldHoldingDiff3.Item1,
                        mixedChangedHoldingDiff4.Item1,
                        mixedOldHoldingDiff6.Item1
                    },
                    new List<Holding>
                    {
                        mixedNewHoldingDiff1.Item1,
                        mixedChangedHoldingDiff2.Item2,
                        mixedChangedHoldingDiff4.Item2,
                        mixedNewHoldingDiff5.Item1,
                    },
                    new List<HoldingDiff>
                    {
                        mixedNewHoldingDiff1.Item2,
                        mixedChangedHoldingDiff2.Item3,
                        mixedOldHoldingDiff3.Item2,
                        mixedChangedHoldingDiff4.Item3,
                        mixedNewHoldingDiff5.Item2,
                        mixedOldHoldingDiff6.Item2
                    }).SetName("{m}_MixedHoldings_ReturnsExpectedHoldingDiffs");
            }
        }

        private static (Holding, HoldingDiff) GetNewHoldingDiff(decimal sharesChange, decimal weightChange, int companyId, int id)
        {
            var holding = new Holding
            {
                Id = id,
                FundId = FundId,
                CompanyId = companyId,
                Weight = weightChange,
                Shares = sharesChange
            };

            var holdingDiff = new HoldingDiff
            {
                FundId = FundId,
                CompanyId = companyId,
                NewHoldingId = id,
                OldShares = 0,
                SharesChange = sharesChange,
                OldWeight = 0,
                WeightChange = weightChange
            };

            return (holding, holdingDiff);
        }

        private static (Holding, HoldingDiff) GetOldHoldingDiff(decimal shares, decimal weight, int companyId, int id)
        {
            var holding = new Holding
            {
                Id = id,
                FundId = FundId,
                CompanyId = companyId,
                Weight = weight,
                Shares = shares
            };

            var holdingDiff = new HoldingDiff
            {
                FundId = FundId,
                CompanyId = companyId,
                OldHoldingId = id,
                OldShares = shares,
                SharesChange = -shares,
                OldWeight = weight,
                WeightChange = -weight
            };

            return (holding, holdingDiff);
        }

        private static (Holding, Holding, HoldingDiff) GetChangedHoldingDiff(decimal oldShares, decimal newShares, decimal oldWeight, decimal newWeight, int companyId, int oldId, int newId)
        {
            var oldHolding = new Holding
            {
                Id = oldId,
                FundId = FundId,
                CompanyId = companyId,
                Weight = oldWeight,
                Shares = oldShares
            };

            var newHolding = new Holding
            {
                Id = newId,
                FundId = FundId,
                CompanyId = companyId,
                Weight = newWeight,
                Shares = newShares
            };

            var holdingDiff = new HoldingDiff
            {
                FundId = FundId,
                CompanyId = companyId,
                OldHoldingId = oldId,
                NewHoldingId = newId,
                OldShares = oldShares,
                SharesChange = newShares - oldShares,
                OldWeight = oldWeight,
                WeightChange = newWeight - oldWeight
            };

            return (oldHolding, newHolding, holdingDiff);
        }

        private static void AssertHoldingDiff(HoldingDiff holdingDiff, HoldingDiff expectedHoldingDiff)
        {
            Assert.Multiple(() =>
            {
                Assert.That(holdingDiff.FundId, Is.EqualTo(expectedHoldingDiff.FundId), "Fund IDs do not match.");
                Assert.That(holdingDiff.CompanyId, Is.EqualTo(expectedHoldingDiff.CompanyId), "Company IDs do not match.");
                Assert.That(holdingDiff.OldHoldingId, Is.EqualTo(expectedHoldingDiff.OldHoldingId), "Old holding IDs do not match.");
                Assert.That(holdingDiff.NewHoldingId, Is.EqualTo(expectedHoldingDiff.NewHoldingId), "New holding IDs do not match.");
                Assert.That(holdingDiff.OldShares, Is.EqualTo(expectedHoldingDiff.OldShares), "Old shares do not match.");
                Assert.That(holdingDiff.SharesChange, Is.EqualTo(expectedHoldingDiff.SharesChange), "Shares changes do not match.");
                Assert.That(holdingDiff.WeightChange, Is.EqualTo(expectedHoldingDiff.WeightChange), "Weight changes do not match.");
            });
        }
    }
}

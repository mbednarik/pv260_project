using FundParser.DAL.Exceptions;
using FundParser.DAL.Models;
using FundParser.DAL.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FundParser.DAL.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        public class GetByIDTests : RepositoryTestsBase
        {
            [Test]
            public async Task GetByID_NonExistingEntity_ReturnsNull()
            {
                // Arrange
                context.Companies.Add(new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                });
                await context.SaveChangesAsync();

                // Act & Assert
                var result = Assert.ThrowsAsync<NotFoundException>(
                    async () => await repository.GetByID(0));

                // Assert
                Assert.That(result.Message,
                    Is.EqualTo($"Cannot find database entity {typeof(Company)} with id 0"));
            }

            [Test]
            public async Task GetByID_ExistingEntity_ReturnsCorrectEntity()
            {
                // Arrange
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };
                context.Companies.Add(expectedEntity);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByID(expectedEntity.Id);

                // Assert
                AssertCompany(result, expectedEntity);
            }
        }

        public class InsertTests : RepositoryTestsBase
        {
            [Test]
            public void Insert_NullEntity_ThrowsException()
            {
                // Act & Assert
                Assert.That(async () => await repository.Insert(null!), Throws.ArgumentNullException);
            }

            [Test]
            public async Task Insert_EmptySet_InsertsEntity()
            {
                // Act
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };
                await repository.Insert(expectedEntity);
                await context.SaveChangesAsync();

                // Assert
                var result = await context.Companies.ToListAsync();

                Assert.That(result, Has.Count.EqualTo(1));
                AssertCompany(result.First(), expectedEntity);
            }

            [Test]
            public async Task Insert_NonEmptySet_InsertsEntity()
            {
                // Arrange
                await context.Companies.AddAsync(new Company
                {
                    Name = CompanyName + 1,
                    Cusip = CompanyCusip + 1,
                    Ticker = CompanyTicker + 1
                });
                await context.SaveChangesAsync();

                // Act
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };
                await repository.Insert(expectedEntity);
                await context.SaveChangesAsync();

                // Assert
                var result = await context.Companies.OrderBy(entity => entity.Id).ToListAsync();

                Assert.That(result, Has.Count.EqualTo(2));
                AssertCompany(result.Skip(1).First(), expectedEntity);
            }
        }

        public class DeleteTests : RepositoryTestsBase
        {
            [Test]
            public void Delete_NonExistingEntity_ThrowsException()
            {
                // Act & Assert
                Assert.That(async () => await repository.Delete(0), Throws.Exception.With.Message.EqualTo("Entity with given Id does not exist."));
            }

            [Test]
            public void Delete_NullEntity_ThrowsException()
            {
                // Act & Assert
                Assert.That(() => repository.Delete(null!), Throws.ArgumentNullException);
            }

            [Test]
            public async Task Delete_OneExistingEntity_DeletesEntity()
            {
                // Arrange
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };
                await context.AddAsync(expectedEntity);
                await context.SaveChangesAsync();

                // Act
                repository.Delete(expectedEntity);
                await context.SaveChangesAsync();

                // Assert
                var result = await context.Companies.ToListAsync();

                Assert.That(result, Is.Empty);
            }

            [Test]
            public async Task Delete_MultipleExistingEntities_DeletesEntity()
            {
                // Arrange
                await context.Companies.AddAsync(new Company
                {
                    Name = CompanyName + 1,
                    Cusip = CompanyCusip + 1,
                    Ticker = CompanyTicker + 1
                });
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };
                await context.AddAsync(expectedEntity);
                await context.SaveChangesAsync();

                // Act
                repository.Delete(expectedEntity);
                await context.SaveChangesAsync();

                // Assert
                var result = await context.Companies.ToListAsync();

                Assert.Multiple(() =>
                {
                    Assert.That(result, Has.Count.EqualTo(1));
                    Assert.That(result.First().Id, Is.Not.EqualTo(expectedEntity.Id));
                });
            }
        }

        public class UpdateTests : RepositoryTestsBase
        {
            [Test]
            public void Update_NullEntity_ThrowsException()
            {
                // Act & Assert
                Assert.That(() => repository.Update(null!), Throws.ArgumentNullException);
            }

            [Test]
            public async Task Update_OneExistingEntity_UpdatesEntity()
            {
                // Arrange
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };
                await context.AddAsync(expectedEntity);
                await context.SaveChangesAsync();

                // Act
                var newName = "newName";
                expectedEntity.Name = newName;
                repository.Update(expectedEntity);
                await context.SaveChangesAsync();

                // Assert
                var result = await context.Companies.ToListAsync();

                AssertCompany(result.Single(), expectedEntity);
            }

            [Test]
            public async Task Update_MultipleExistingEntities_UpdatesCorrectEntity()
            {
                // Arrange
                var otherEntity = new Company
                {
                    Name = CompanyName + 1,
                    Cusip = CompanyCusip + 1,
                    Ticker = CompanyTicker + 1
                };
                var expectedEntity = new Company
                {
                    Name = CompanyName,
                    Cusip = CompanyCusip,
                    Ticker = CompanyTicker
                };

                await context.AddRangeAsync(otherEntity, expectedEntity);
                await context.SaveChangesAsync();

                // Act
                var newName = "newName";
                expectedEntity.Name = newName;
                repository.Update(expectedEntity);
                await context.SaveChangesAsync();

                // Assert
                var result = await context.Companies.OrderBy(entity => entity.Id).ToListAsync();

                Assert.That(result, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    AssertCompany(result.First(), otherEntity);
                    AssertCompany(result.Last(), expectedEntity);
                });
            }

        }

        public class GetAllTests : RepositoryTestsBase
        {
            [Test]
            public async Task GetAll_EmptySet_ReturnsNoEntities()
            {
                // Act
                var result = await repository.GetAll();

                // Assert
                Assert.That(result, Is.Empty);
            }

            [Test]
            public async Task GetAll_NonEmptySet_ReturnsCorrectEntities()
            {
                // Arrange
                var entity1 = new Company
                {
                    Name = CompanyName + 1,
                    Cusip = CompanyCusip + 1,
                    Ticker = CompanyTicker + 1
                };
                var entity2 = new Company
                {
                    Name = CompanyName + 2,
                    Cusip = CompanyCusip + 2,
                    Ticker = CompanyTicker + 2
                };
                var entity3 = new Company
                {
                    Name = CompanyName + 3,
                    Cusip = CompanyCusip + 3,
                    Ticker = CompanyTicker + 3
                };
                await context.AddRangeAsync(entity1, entity2, entity3);
                await context.SaveChangesAsync();

                // Act
                var result = (await repository.GetAll()).OrderBy(entity => entity.Id).ToList();

                // Assert
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.Multiple(() =>
                {
                    AssertCompany(result[0], entity1);
                    AssertCompany(result[1], entity2);
                    AssertCompany(result[2], entity3);
                });
            }
        }

        public class GetQueryableTests : RepositoryTestsBase
        {
            [Test]
            public void GetQueryable_EmptySet_ReturnsNoEntities()
            {
                // Act
                var result = repository.GetQueryable().ToList();

                // Assert
                Assert.That(result, Is.Empty);
            }

            [Test]
            public async Task GetQueryable_NonEmptySet_ReturnsCorrectEntities()
            {
                // Arrange
                var entity1 = new Company
                {
                    Name = CompanyName + 1,
                    Cusip = CompanyCusip + 1,
                    Ticker = CompanyTicker + 1
                };
                var entity2 = new Company
                {
                    Name = CompanyName + 2,
                    Cusip = CompanyCusip + 2,
                    Ticker = CompanyTicker + 2
                };
                var entity3 = new Company
                {
                    Name = CompanyName + 3,
                    Cusip = CompanyCusip + 3,
                    Ticker = CompanyTicker + 3
                };
                await context.AddRangeAsync(entity1, entity2, entity3);
                await context.SaveChangesAsync();

                // Act
                var result = repository.GetQueryable().OrderBy(entity => entity.Id).ToList();

                // Assert
                Assert.That(result, Has.Count.EqualTo(3));
                Assert.Multiple(() =>
                {
                    AssertCompany(result[0], entity1);
                    AssertCompany(result[1], entity2);
                    AssertCompany(result[2], entity3);
                });
            }
        }

        [TestFixture]
        public class RepositoryTestsBase
        {
            protected const string CompanyName = "Name";
            protected const string CompanyCusip = "Cusip";
            protected const string CompanyTicker = "Ticker";

            protected FundParserDbContext context;
            protected Repository<Company> repository;

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

                context = new FundParserDbContext(options);
                repository = new Repository<Company>(context);
            }

            [TearDown]
            public void TearDown()
            {
                context.Dispose();
            }

            protected static void AssertCompany(Company? company, Company expectedCompany)
            {
                Assert.That(company, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(company.Name, Is.EqualTo(expectedCompany.Name), "Company name does not match.");
                    Assert.That(company.Cusip, Is.EqualTo(expectedCompany.Cusip), "Company cusip does not match.");
                    Assert.That(company.Ticker, Is.EqualTo(expectedCompany.Ticker), "Company ticket does not match.");
                });
            }
        }
    }
}

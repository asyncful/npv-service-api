using System.Net;
using Microsoft.Azure.Cosmos;
using Moq;
using NetPresentValueService.Domain.Features.DiscountRates;
using NetPresentValueService.Infrastructure.DataPersistence;
using NetPresentValueService.Infrastructure.Features.DiscountRates;

namespace NetPresentValueService.Infrastructure.Test.Features.DiscountRates;

public class DiscountRateRepositoryTests
{
    private readonly Mock<ICosmosContainerProvider> _containerProviderMock;
    private readonly Mock<Container> _containerMock;
    private readonly DiscountRateRepository _repository;

    private const string DbName = "TestDb";
    private const string ContainerName = "DiscountRates";
    private const string PartitionKey = "IncrementedDiscountDetailsPartition";
    private const string SettingsId = "default";

    public DiscountRateRepositoryTests()
    {
        _containerMock = new Mock<Container>();
        _containerProviderMock = new Mock<ICosmosContainerProvider>();
        _containerProviderMock
            .Setup(p => p.GetContainer(DbName, ContainerName))
            .Returns(_containerMock.Object);

        _repository = new DiscountRateRepository(_containerProviderMock.Object, DbName, ContainerName);
    }

    [Fact]
    public async Task GetAsync_ReturnsMappedDomainObject_WhenDocumentExists()
    {
        // Arrange
        var userId = "user123";
        var doc = new IncrementedDiscountRateDetailsDocument
        {
            Id = userId,
            PartitionKey = userId,
            LowerBound = 0.01m,
            UpperBound = 0.05m,
            Increment = 0.01m
        };

        var mockResponse = new Mock<ItemResponse<IncrementedDiscountRateDetailsDocument>>();
        mockResponse.Setup(r => r.Resource).Returns(doc);

        _containerMock
            .Setup(c => c.ReadItemAsync<IncrementedDiscountRateDetailsDocument>(
                userId,
                It.Is<PartitionKey>(p => p.ToString().Contains(userId)),
                null,
                default))
            .ReturnsAsync(mockResponse.Object);

        // Act
        var result = await _repository.GetAsync(userId);

        // Assert
        Assert.Equal(0.01m, result.LowerBoundDiscountRate.Value);
        Assert.Equal(0.05m, result.UpperBoundDiscountRate.Value);
        Assert.Equal(0.01m, result.Increment.Value);
    }

    [Fact]
    public async Task GetAsync_WhenNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = "user123";
        _containerMock
            .Setup(c => c.ReadItemAsync<IncrementedDiscountRateDetailsDocument>(
                userId,
                It.IsAny<PartitionKey>(),
                null,
                default))
            .ThrowsAsync(new CosmosException("Not found", HttpStatusCode.NotFound, 0, "", 0));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetAsync(userId));
    }

    [Fact]
    public async Task SaveAsync_MapsDomainAndCallsUpsertItemAsync()
    {
        // Arrange
        var userId = "user123";
        var domain = new IncrementedDiscountRateDetails(
            new DiscountRate(0.01m),
            new DiscountRate(0.05m),
            new DiscountRate(0.01m)
        );

        _containerMock
            .Setup(c => c.UpsertItemAsync(
                It.Is<IncrementedDiscountRateDetailsDocument>(doc =>
                    doc.Id == userId &&
                    doc.PartitionKey == userId &&
                    doc.LowerBound == 0.01m &&
                    doc.UpperBound == 0.05m &&
                    doc.Increment == 0.01m),
                It.Is<PartitionKey>(pk => pk.ToString().Contains(userId)),
                null,
                default))
            .ReturnsAsync(Mock.Of<ItemResponse<IncrementedDiscountRateDetailsDocument>>());

        // Act
        await _repository.SaveAsync(userId, domain);

        // Assert
        _containerMock.Verify(c => c.UpsertItemAsync(
            It.IsAny<IncrementedDiscountRateDetailsDocument>(),
            It.IsAny<PartitionKey>(),
            null,
            default), Times.Once);
    }
}
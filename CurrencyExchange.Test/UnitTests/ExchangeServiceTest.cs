using CurrencyExchange.ApplicationCore.Entities;
using CurrencyExchange.ApplicationCore.Exceptions;
using CurrencyExchange.ApplicationCore.Interfaces;
using CurrencyExchange.ApplicationCore.Services;
using Moq; 

namespace CurrencyExchange.Test.UnitTests;

[TestClass]
public class ExchangeServiceTest
{
    private readonly Mock<IFrankFurterClient> _mockFrankFurterClient;
    private readonly Mock<IConversionHistoryRepository> _mockConversionHistoryRepository;
    private readonly Mock<IRedisService> _mockRedisService;
    private readonly Mock<ICurrencyPairRepository> _mockCurrencyPairRepository;

    public ExchangeServiceTest()
    {
        _mockConversionHistoryRepository = new Mock<IConversionHistoryRepository>();
        _mockFrankFurterClient = new Mock<IFrankFurterClient>();
        _mockRedisService = new Mock<IRedisService>();
        _mockCurrencyPairRepository = new Mock<ICurrencyPairRepository>();
    }

    [DataRow(100, "USD", "EUR", 92.456, 0.92456)]
    [DataRow(20, "GBP", "USD", 25.222, 1.2611)]
    [DataRow(15, "GBP", "EUR", 17.4885, 1.1659)]
    [TestMethod]
    public async Task Convert_Valid(int amount, string baseCurrency, string target, double expected, double rate)
    {
        // Arrange
        var currencyPair = new CurrencyPairEntity { Base = baseCurrency, Target = target, Rate = rate };

        _mockRedisService.Setup(s => s.GetCachedDataAsync<CurrencyPairEntity>(It.IsAny<string>()))
                         .ReturnsAsync(currencyPair);

        var exchangeService = new ExchangeService(_mockFrankFurterClient.Object,
                                              _mockConversionHistoryRepository.Object,
                                              _mockRedisService.Object,
                                              _mockCurrencyPairRepository.Object);
        // Act
        var result = await exchangeService.Convert(baseCurrency, target, amount);
        
        // Assert
        Assert.AreEqual(expected, result.Amount);
    }

    [DataRow(100, "USD", "123")]
    [DataRow(100, "123", "EUR")]
    [DataRow(100, "123", "123")]
    [TestMethod]
    public async Task Convert_Invalid_Code(int amount, string baseCurrency, string target)
    {
        // Arrange
        var exchangeService = new ExchangeService(_mockFrankFurterClient.Object,
                                              _mockConversionHistoryRepository.Object,
                                              _mockRedisService.Object,
                                              _mockCurrencyPairRepository.Object);
        
        // Assert
        await Assert.ThrowsExceptionAsync<InvalidCodeException>(async () =>  
              await exchangeService.Convert(baseCurrency, target, amount)
        );
    }

    [TestMethod]
    public async Task GetRatesHistory_Valid()
    {
        // Arrange
        var conversionHistoryList = new List<ConversionHistory>()
        {
            new ConversionHistory
            {
                Amount = 100,
                Base = "USD",
                Rate = 0.9,
                Created = DateTime.Now,
                Id = 1,
                LastModified = DateTime.Now,
                Target = "EUR",
                TargetAmount = 92.3
            }
        };

        _mockConversionHistoryRepository.Setup(r => r.GetRatesHistoryListAsync(It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(conversionHistoryList);

        var exchangeService = new ExchangeService(_mockFrankFurterClient.Object,
                                             _mockConversionHistoryRepository.Object,
                                             _mockRedisService.Object,
                                             _mockCurrencyPairRepository.Object);
        
        // Act
        var result = await exchangeService.GetRatesHistory();
        
        // Assert
        Assert.IsTrue(result.Any());
    }

    [TestMethod]
    public async Task GetRatesHistory_Empty_List()
    {
        // Arrange
        var conversionHistoryList = new List<ConversionHistory>();

        _mockConversionHistoryRepository.Setup(r => r.GetRatesHistoryListAsync(It.IsAny<CancellationToken>()))
                                        .ReturnsAsync(conversionHistoryList);

        var exchangeService = new ExchangeService(_mockFrankFurterClient.Object,
                                              _mockConversionHistoryRepository.Object,
                                              _mockRedisService.Object,
                                              _mockCurrencyPairRepository.Object);

        // Assert
        await Assert.ThrowsExceptionAsync<EmptyConversionHistoryException>(async () => 
            await exchangeService.GetRatesHistory()
        );
    }
}

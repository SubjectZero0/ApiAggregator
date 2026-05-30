using FluentAssertions;
using Gateway.Models;
using Gateway.Utils.Mappers;

namespace GatewayTests.Utils.Mappers;

public class MarketMapperTests
{
	private readonly MarketMapper _sut = new();

	[Fact]
	public void Map_MapsAllFields()
	{
		//Arrange
		var tickers = new[]
		{
			new MassiveStockTicker
			{
				Ticker = "AAPL",
				ClosePrice = 150.5,
				HighPrice = 155.0,
				LowPrice = 148.0,
				TransactionCount = 1000,
				OpenPrice = 149.0,
				Volume = 500000
			}
		};

		//Act
		var result = _sut.Map(tickers);

		//Assert
		result.Should().HaveCount(1);
		result[0].Ticker.Should().Be("AAPL");
		result[0].ClosePrice.Should().Be(150.5);
		result[0].HighPrice.Should().Be(155.0);
		result[0].LowPrice.Should().Be(148.0);
		result[0].TransactionCount.Should().Be(1000);
		result[0].OpenPrice.Should().Be(149.0);
		result[0].Volume.Should().Be(500000);
	}

	[Fact]
	public void Map_EmptyInput_ReturnsEmpty()
	{
		//Act
		var result = _sut.Map([]);

		//Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public void Map_MultipleItems_MapsAll()
	{
		//Arrange
		var tickers = new[]
		{
			new MassiveStockTicker { Ticker = "AAPL" },
			new MassiveStockTicker { Ticker = "MSFT" },
		};

		//Act
		var result = _sut.Map(tickers);

		//Assert
		result.Should().HaveCount(2);
		result.Select(r => r.Ticker).Should().BeEquivalentTo(["AAPL", "MSFT"]);
	}
}
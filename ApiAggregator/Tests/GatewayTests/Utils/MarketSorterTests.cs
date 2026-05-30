using Application.Models.Finance;
using FluentAssertions;
using Gateway.Models;
using Gateway.Utils;

namespace GatewayTests.Utils;

public class MarketSorterTests
{
	private readonly MarketSorter _sut = new();

	private static MassiveStockTicker[] BuildMarkets() =>
	[
		new() { Ticker = "A", HighPrice = 10, Volume = 100 },
		new() { Ticker = "B", HighPrice = 30, Volume = 300 },
		new() { Ticker = "C", HighPrice = 20, Volume = 200 },
	];

	[Fact]
	public void Sort_WithNone_ReturnFirstN()
	{
		//Arrange
		var markets = BuildMarkets();

		//Act
		var result = _sut.Sort(markets, 2, FieldOrdering.None, FieldToSort.HighestPrice);

		//Assert
		result.Should().HaveCount(2);
		result.First().Ticker.Should().Be("A");
	}

	[Fact]
	public void Sort_ByHighestPriceDescending_ReturnsSortedDescending()
	{
		//Arrange
		var markets = BuildMarkets();

		//Act
		var result = _sut.Sort(markets, 3, FieldOrdering.Descending, FieldToSort.HighestPrice).ToList();

		//Assert
		result[0].Ticker.Should().Be("B");
		result[1].Ticker.Should().Be("C");
		result[2].Ticker.Should().Be("A");
	}

	[Fact]
	public void Sort_ByHighestPriceAscending_ReturnsSortedAscending()
	{
		//Arrange
		var markets = BuildMarkets();

		//Act
		var result = _sut.Sort(markets, 3, FieldOrdering.Ascending, FieldToSort.HighestPrice).ToList();

		//Assert
		result[0].Ticker.Should().Be("A");
		result[1].Ticker.Should().Be("C");
		result[2].Ticker.Should().Be("B");
	}

	[Fact]
	public void Sort_ByVolumeDescending_ReturnsSortedByVolumeDescending()
	{
		//Arrange
		var markets = BuildMarkets();

		//Act
		var result = _sut.Sort(markets, 3, FieldOrdering.Descending, FieldToSort.Volume).ToList();

		//Assert
		result[0].Ticker.Should().Be("B");
		result[1].Ticker.Should().Be("C");
		result[2].Ticker.Should().Be("A");
	}

	[Fact]
	public void Sort_LimitsResultToNumberOfMarkets()
	{
		//Arrange
		var markets = BuildMarkets();

		//Act
		var result = _sut.Sort(markets, 1, FieldOrdering.Descending, FieldToSort.HighestPrice);

		//Assert
		result.Should().HaveCount(1);
		result.Single().Ticker.Should().Be("B");
	}
}
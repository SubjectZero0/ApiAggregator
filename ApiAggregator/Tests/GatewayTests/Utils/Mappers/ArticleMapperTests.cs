using FluentAssertions;
using Gateway.Models;
using Gateway.Utils.Mappers;

namespace GatewayTests.Utils.Mappers;

public class ArticleMapperTests
{
	private readonly ArticleMapper _sut = new();

	[Fact]
	public void Map_MapsAllFields()
	{
		//Arrange
		var articles = new[]
		{
			new Article
			{
				Source = new Source { Name = "BBC" },
				Author = "John",
				Title = "Title",
				Description = "Desc",
				Url = "http://bbc.com",
				UrlToImage = "http://bbc.com/img.png",
				Content = "Content"
			}
		};

		//Act
		var result = _sut.Map(articles);

		//Assert
		result.Should().HaveCount(1);
		result[0].SourceName.Should().Be("BBC");
		result[0].Author.Should().Be("John");
		result[0].Title.Should().Be("Title");
		result[0].Description.Should().Be("Desc");
		result[0].Url.Should().Be("http://bbc.com");
		result[0].UrlToImage.Should().Be("http://bbc.com/img.png");
		result[0].Content.Should().Be("Content");
	}

	[Fact]
	public void Map_WhenSourceIsNull_UsesEmptyString()
	{
		//Arrange
		var articles = new[] { new Article { Source = null } };

		//Act
		var result = _sut.Map(articles);

		//Assert
		result[0].SourceName.Should().BeEmpty();
	}

	[Fact]
	public void Map_EmptyInput_ReturnsEmpty()
	{
		//Act
		var result = _sut.Map([]);

		//Assert
		result.Should().BeEmpty();
	}
}
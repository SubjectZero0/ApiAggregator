namespace Application.Caching
{
	public interface ICachingProvider
	{
		Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan expiration);

		Task RemoveAsync(string key);
	}
}
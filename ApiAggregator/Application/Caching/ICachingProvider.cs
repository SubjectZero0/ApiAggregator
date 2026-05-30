namespace Application.Caching
{
	public interface ICachingProvider
	{
		Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, string cacheName);

		Task RemoveAsync(string key);
	}
}
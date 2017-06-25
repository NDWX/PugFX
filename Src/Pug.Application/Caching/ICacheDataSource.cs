namespace Pug.Application.Caching
{
	public interface ICacheDataSource<K, V>
	{
		V Get(K key);
	}

}

using Godot.Collections;


public class ArrayHashMap<K, V> : Godot.Object
{
	public V this[K key]
	{
		get
		{
			return map[key];
		}
		set
		{
			map[key] = value;
		}
	}

	public void Add(K key, V value)
	{
		map.Add(key, value);
		keyList.Add(key);
	}

	public V Get(K key)
	{
		V value;

		if(map.TryGetValue(key, out value))
			return value;

		return default(V);
	}

	public V GetAt(int index)
	{
		return Get(GetKeyAt(index));
	}

	public K GetKeyAt(int index)
	{
		return keyList[index];
	}

	public void RemoveAt(int index)
	{
		map.Remove(keyList[index]);
		keyList.RemoveAt(index);
	}

	public bool ContainsKey(K key)
	{
		return map.ContainsKey(key);
	}

	public void Clear()
	{
		map.Clear();
		keyList.Clear();
	}

	public int Count
	{
		get
		{
			return map.Count;
		}
	}

	public ArrayHashMap()
	{
		map = new Dictionary<K, V>();
		keyList = new Array<K>();
	}

	
	private Dictionary<K, V> map;
	private Array<K> keyList;
}

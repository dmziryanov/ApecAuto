using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmsAuto.TechDoc.Cache
{
	public class BoundedMap<TKey, TValue>
	{
		class BoundedMapEntry
		{
			public readonly TValue Value;
			public readonly LinkedListNode<TKey> KeyNode;

			public BoundedMapEntry(TValue value, LinkedListNode<TKey> keyNode)
			{
				Value = value;
				KeyNode = keyNode;
			}
		}

		private int _capacity;
		private Dictionary<TKey, BoundedMapEntry> _innerDictionary;
		private LinkedList<TKey> _keys;
		private readonly object _sync = new object();

		public BoundedMap(int capacity)
		{
			if (capacity <= 0)
				throw new ArgumentOutOfRangeException("capacity", "Capacity must be positive");
			_capacity = capacity;
			_innerDictionary = new Dictionary<TKey, BoundedMap<TKey, TValue>.BoundedMapEntry>(capacity);
			_keys = new LinkedList<TKey>();
		}

		public TValue this[TKey key]
		{
			get
			{
				EnsureKeyNotNull(key);

				TValue result = default(TValue);
				if (_innerDictionary.ContainsKey(key))
					lock (_sync)
						if (_innerDictionary.ContainsKey(key))
						{
							BoundedMapEntry entry = _innerDictionary[key];
							if (entry.KeyNode.Next != null)
							{
								_keys.Remove(entry.KeyNode);
								_keys.AddLast(entry.KeyNode);
							}
							result = entry.Value;
						}
				return result;
			}
		}

		public IEnumerable<TValue> Values
		{
			get
			{
				foreach (TKey key in _keys)
					yield return _innerDictionary[key].Value;
			}
		}

		public void Add(TKey key, TValue value)
		{
			EnsureKeyNotNull(key);

			if (!_innerDictionary.ContainsKey(key))
				lock (_sync)
					if (!_innerDictionary.ContainsKey(key))
					{
						LinkedListNode<TKey> keyNode = null;
						if (_innerDictionary.Count < _capacity)
						{
							keyNode = new LinkedListNode<TKey>(key);
						}
						else
						{
							keyNode = _keys.First;
							_innerDictionary.Remove(keyNode.Value);
							_keys.RemoveFirst();
							keyNode.Value = key;
						}

						_innerDictionary.Add(key, new BoundedMap<TKey, TValue>.BoundedMapEntry(value, keyNode));
						_keys.AddLast(keyNode);

						return;
					}
			throw new InvalidOperationException("Key already exists");
		}

		public void Remove(TKey key)
		{
			EnsureKeyNotNull(key);
			if (_innerDictionary.ContainsKey(key))
				lock (_sync)
					if (_innerDictionary.ContainsKey(key))
					{
						BoundedMapEntry entry = _innerDictionary[key];
						_innerDictionary.Remove(key);
						_keys.Remove(entry.KeyNode);
					}
		}

		public bool ContainsKey(TKey key)
		{
			EnsureKeyNotNull(key);
			return _innerDictionary.ContainsKey(key);
		}

		private void EnsureKeyNotNull(TKey key)
		{
			if (key == null)
				throw new ArgumentNullException("key", "Key cannot be null");
		}
	}
}

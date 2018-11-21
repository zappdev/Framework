﻿using System.Collections.Generic;
using System.Threading;

namespace CLMS.Framework.Utilities
{
	public class ThreadLocalStorage
	{
		private static readonly ThreadLocal<Dictionary<string, object>> Items 
			= new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());

		public static object Get<T>(string key)
		{
			return Items.Value.ContainsKey(key) ? (T)Items.Value[key] : default(T);
		}

		public static void Remove(string key)
		{
			if (Items.Value.ContainsKey(key))
			{
				Items.Value.Remove(key);
			}
		}

		public static void Set(string key, object obj)
		{
			if (Items.Value.ContainsKey(key))
			{
				// Update
				Items.Value[key] = obj;
			}
			else
			{
				Items.Value.Add(key, obj);
			}
		}
	}
}
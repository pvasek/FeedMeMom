using System;
using System.Collections.Generic;


namespace FeedMeMom.Common
{
	public static class ServiceLocator
	{
		static ServiceLocator()
		{
			//Register (Repository.CreatePersonalDb ("feedmemom_data"));
			//Register (new AppService());
			_instances = new Dictionary<Type, object>();
			_instances.Add(typeof(Repository), Repository.CreatePersonalDb("feedmemom_data"));
			_instances.Add(typeof(AppService), new AppService());
		}

		//private static Dictionary<Type, Func<object>> _factoryMethods = new Dictionary<Type,Func<object>>();
		private static Dictionary<Type, object> _instances;
		
		public static T Get<T> ()
		{
			//return (T) _factoryMethods [typeof(T)] ();
			return (T)_instances[typeof(T)];
		}
		
		public static void Register<T> (T instance)
		{
			_instances.Add (typeof(T), instance);
		}
		
//		public static void Register<T> (Func<T> factoryMethod) where T: class
//		{
//			_factoryMethods.Add (typeof(T), factoryMethod);
//		}

		public static void Dispose()
		{
			foreach (var instance in _instances.Values)
			{
				var disposable = instance as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			_instances = null;
		}
	}
}


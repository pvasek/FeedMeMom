using System;
using System.Collections.Generic;


namespace FeedMeMom.Common
{
	public static class ServiceLocator
	{
		static ServiceLocator()
		{
			Register (Repository.CreatePersonalDb ("feedmemom_data"));
			Register (new AppService());
		}

		private static Dictionary<Type, Func<object>> _factoryMethods = new Dictionary<Type,Func<object>>();
		
		public static T Get<T> ()
		{
			return (T) _factoryMethods [typeof(T)] ();
		}
		
		public static void Register<T> (T instance)
		{
			_factoryMethods.Add (typeof(T), () => instance);
		}
		
		public static void Register<T> (Func<T> factoryMethod) where T: class
		{
			_factoryMethods.Add (typeof(T), factoryMethod);
		}
	}
}


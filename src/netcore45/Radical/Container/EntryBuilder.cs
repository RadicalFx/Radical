using System;
using System.Reflection;
using Topics.Radical.ComponentModel;

namespace Topics.Radical
{
	/// <summary>
	/// Helper class to fluently create Puzzle container entries.
	/// </summary>
	public static class EntryBuilder
	{
		/// <summary>
		/// Creates a new container entry for the specified type.
		/// </summary>
		/// <param name="type">The type to create entry for.</param>
		/// <returns>The container entry.</returns>
		public static IPuzzleContainerEntry For( TypeInfo type )
		{
			if( type.IsInterface )
			{
				return new PuzzleContainerEntry<Object>() { Service = type };
			}
			else
			{
				return new PuzzleContainerEntry<Object>() { Component = type };
			}
		}

		/// <summary>
		/// Creates a new container entry for the specified type.
		/// </summary>
		/// <typeparam name="T">The type to create entry for.</typeparam>
		/// <returns>
		/// The container entry.
		/// </returns>
		public static IPuzzleContainerEntry<T> For<T>()
		{
			var type = typeof( T ).GetTypeInfo();
			if( type.IsInterface )
			{
				return new PuzzleContainerEntry<T>() { Service = type };
			}
			else
			{
				return new PuzzleContainerEntry<T>() { Component = type };
			}
		}
	}
}

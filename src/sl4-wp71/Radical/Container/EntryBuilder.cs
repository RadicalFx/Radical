using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
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
		public static IPuzzleContainerEntry For( Type type )
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
			var type = typeof( T );
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

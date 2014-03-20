using System;
using System.Linq.Expressions;

namespace Topics.Radical.Linq
{
	/// <summary>
	/// Extends the expression class.
	/// </summary>
	public static class ExpressionExtensions
	{
		/// <summary>
		/// Gets the name of the member.
		/// </summary>
		/// <typeparam name="T">The member type.</typeparam>
		/// <param name="source">The source expression that represents the member.</param>
		/// <returns>The name of the member.</returns>
		public static String GetMemberName<T>( this Expression<Func<T>> source )
		{
			var expression = source.Body as MemberExpression;
			if( expression != null )
			{
				var member = expression.Member;
				return member.Name;
			}

			throw new NotSupportedException( "Only MemberExpression(s) are supported." );
		}

		/// <summary>
		/// Gets the name of the member.
		/// </summary>
		/// <typeparam name="T">The type of object that expose the member.</typeparam>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>The name of the member.</returns>
		public static String GetMemberName<T, TProperty>( this Expression<Func<T, TProperty>> source )
		{
			var expression = FindMemberExpression( source.Body );
			var member = expression.Member;

			return member.Name;
		}

		static MemberExpression FindMemberExpression( Expression exp )
		{
			if( exp is MemberExpression )
			{
				return ( MemberExpression )exp;
			}

			if( exp is UnaryExpression )
			{
				return FindMemberExpression( ( ( UnaryExpression )exp ).Operand );
			}

			throw new NotSupportedException( "The supplied expression type is not supported." );
		}
	}
}

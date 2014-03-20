using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Topics.Radical.Helpers
{
	/// <summary>
	/// Helper class to manage passwords.
	/// </summary>
	public static class Password
	{
		/// <summary>
		/// Creates a random salt.
		/// </summary>
		/// <returns>The randomly created salt.</returns>
		public static Byte[] CreateRandomSalt()
		{
			var saltBytes = new Byte[ 4 ];
			var rng = new RNGCryptoServiceProvider();
			rng.GetBytes( saltBytes );

			return saltBytes;
		}

		/// <summary>
		/// Creates the hash (SHA1) of the given password using the supplied salt.
		/// </summary>
		/// <param name="clearTextPassword">The clear text password.</param>
		/// <param name="passwordSalt">The password salt.</param>
		/// <returns>The hash of the given password.</returns>
		public static Byte[] CreateHash( String clearTextPassword, Byte[] passwordSalt )
		{
			return Password.CreateHash( clearTextPassword, passwordSalt, "SHA1" );
		}

		/// <summary>
		/// Creates the hash of the given password using the supplied salt.
		/// </summary>
		/// <param name="clearTextPassword">The clear text password.</param>
		/// <param name="passwordSalt">The password salt.</param>
		/// <param name="hashAlgorithmName">Name of the hash algorithm.</param>
		/// <returns>
		/// The hash of the given password.
		/// </returns>
		public static Byte[] CreateHash( String clearTextPassword, Byte[] passwordSalt, String hashAlgorithmName )
		{
			var bytes = Encoding.Unicode.GetBytes( clearTextPassword );
			var buffer = new byte[ passwordSalt.Length + bytes.Length ];

			Buffer.BlockCopy( passwordSalt, 0, buffer, 0, passwordSalt.Length );
			Buffer.BlockCopy( bytes, 0, buffer, passwordSalt.Length, bytes.Length );

			var hash = HashAlgorithm.Create( hashAlgorithmName ).ComputeHash( buffer );

			return hash;
		}
	}
}

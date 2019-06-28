namespace Radical.ComponentModel
{
    using System;
    using System.Text;

    /// <summary>
    /// A concrete implementazion of a timestamp type whose underlying type
    /// is an array of <c>Byte</c>.
    /// </summary>
    public sealed class ByteArrayTimestamp : Timestamp<Byte[]>
    {
        /// <summary>
        /// An empty <c>ByteArrayTimestamp</c> instance, exposed as a singleton.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes" )]
        public static readonly ByteArrayTimestamp Empty = new Byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayTimestamp"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ByteArrayTimestamp( Byte[] value )
            : base( value )
        {

        }

        /// <summary>
        /// Determines whether the specified <see cref="T:Timestamp"/> is equal to the current <see cref="T:Timestamp"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:Timestamp"/> to compare with the current <see cref="T:Timestamp"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:Timestamp"/> is equal to the current <see cref="T:Timestamp"/>; otherwise, false.
        /// </returns>
        public override bool Equals( Timestamp obj )
        {
            bool returnValue = true;

            ByteArrayTimestamp timestamp = obj as ByteArrayTimestamp;
            if( obj != null )
            {
                if( this.Value.Length != timestamp.Value.Length )
                {
                    returnValue = false;
                }
                else
                {
                    for( int i = 0; i < timestamp.Value.Length; i++ )
                    {
                        if( this.Value[ i ] != timestamp.Value[ i ] )
                        {
                            returnValue = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }

        /// <summary>
        /// Performs an implicit conversion from <c>Byte[]</c> to <see cref="Radical.ComponentModel.ByteArrayTimestamp"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ByteArrayTimestamp( Byte[] value )
        {
            return new ByteArrayTimestamp( value );
        }

        /// <summary>
        /// Returns a <see cref="T:System.string"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.string"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach( var b in this.Value )
            {
                sb.AppendFormat( "{0:X2}:", b );
            }

            return sb.ToString().TrimEnd( new[] { ':' } );
        }
    }
}
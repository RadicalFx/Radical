using Radical.Validation;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Radical
{
    /// <summary>
    /// Represents a writer that can write a sequential series of characters using an 
    /// Action delegate ad the destination.
    /// </summary>
    public sealed class ActionTextWriter : TextWriter
    {
        readonly Action<string> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTextWriter"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ActionTextWriter(Action<string> logger)
            : base(CultureInfo.InvariantCulture)
        {
            Ensure.That(logger).Named("logger").IsNotNull();

            this.logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTextWriter"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="formatProvider">The format provider.</param>
        public ActionTextWriter(Action<string> logger, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            Ensure.That(logger).Named("logger").IsNotNull();

            this.logger = logger;
        }

        /// <summary>
        /// Writes a string to the text stream.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:System.IO.TextWriter"/> is closed.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        public override void Write(string value)
        {
            this.logger(value);
        }

        /// <summary>
        /// Writes a subarray of characters to the text stream.
        /// </summary>
        /// <param name="buffer">The character array to write data from.</param>
        /// <param name="index">Starting index in the buffer.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <exception cref="T:System.ArgumentException">
        /// The buffer length minus <paramref name="index"/> is less than <paramref name="count"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="buffer"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="index"/> or <paramref name="count"/> is negative.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">
        /// The <see cref="T:System.IO.TextWriter"/> is closed.
        /// </exception>
        /// <exception cref="T:System.IO.IOException">
        /// An I/O error occurs.
        /// </exception>
        public override void Write(char[] buffer, int index, int count)
        {
            if (buffer == null || index < 0 || count < 0 || buffer.Length - index < count)
            {
                //let base class to throw exception
                base.Write(buffer, index, count);
            }

            this.logger(new string(buffer, index, count));
        }

        Encoding _encoding;

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="T:System.Text.Encoding"/> in which the output is written.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The Encoding in which the output is written.
        /// </returns>
        public override Encoding Encoding
        {
            get
            {
                if (this._encoding == null)
                {
                    this._encoding = new UnicodeEncoding(false, false);
                }

                return this._encoding;
            }
        }
    }
}

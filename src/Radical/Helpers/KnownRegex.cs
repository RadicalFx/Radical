using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.Helpers
{
    public static class KnownRegex
    {
        public static class CreditCards
        {
            /// <summary>
            /// All Visa card numbers start with a 4. New cards have 16 digits. Old cards have 13.
            /// </summary>
            public static readonly String Visa = @"^4[0-9]{12}(?:[0-9]{3})?$";

            /// <summary>
            /// All MasterCard numbers start with the numbers 51 through 55. All have 16 digits.
            /// </summary>
            public static readonly String MasterCard = @"^5[1-5][0-9]{14}$";

            /// <summary>
            /// American Express card numbers start with 34 or 37 and have 15 digits.
            /// </summary>
            public static readonly String AmericanExpress = @"^3[47][0-9]{13}$";

            /// <summary>
            /// Diners Club card numbers begin with 300 through 305, 36 or 38. All have 14 digits. There are Diners Club cards that begin with 5 and have 16 digits. These are a joint venture between Diners Club and MasterCard, and should be processed like a MasterCard.
            /// </summary>
            public static readonly String Diners = @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$";

            /// <summary>
            /// Discover card numbers begin with 6011 or 65. All have 16 digits.
            /// </summary>
            public static readonly String Discover = @"^6(?:011|5[0-9]{2})[0-9]{12}$";

            /// <summary>
            /// JCB cards beginning with 2131 or 1800 have 15 digits. JCB cards beginning with 35 have 16 digits.
            /// </summary>
            public static readonly String JCB = @"^(?:2131|1800|35\d{3})\d{11}$";
        }

        /// <summary>
        /// Validates an email address
        /// </summary>
        public static readonly String MailAddress = @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$";
        
        /// <summary>
        /// validates an url, http, https or ftp.
        /// </summary>
        public static readonly String Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";
    }
}

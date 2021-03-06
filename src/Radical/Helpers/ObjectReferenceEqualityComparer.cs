﻿using System.Collections.Generic;

namespace Radical
{
    static class ObjectReferenceEqualityComparer
    {
        private static readonly IEqualityComparer<object> _instance = new ReferenceEqualityComparer<object>();
        public static IEqualityComparer<object> Instance
        {
            get { return _instance; }
        }
    }
}

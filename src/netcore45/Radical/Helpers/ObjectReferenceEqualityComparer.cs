namespace Topics.Radical
{
	using System;
	using System.Collections.Generic;

	static class ObjectReferenceEqualityComparer
	{
		private static IEqualityComparer<Object> _instance = new ReferenceEqualityComparer<Object>();
		public static IEqualityComparer<Object> Instance
		{
			get { return _instance; }
		}
	}
}

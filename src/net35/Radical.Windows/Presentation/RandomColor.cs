using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Reflection;

namespace Topics.Radical.Windows
{
	public sealed class RandomSolidColorBrush
	{
		Random random;
		PropertyInfo[] _props;
		int _MaxProps;

		public RandomSolidColorBrush()
		{
			random = new Random((int)DateTime.Now.Ticks);
			_props = typeof(Brushes).GetProperties();
			_MaxProps = _props.Length;
		}

		public SolidColorBrush Next()
		{
			var number = random.Next(_MaxProps);
			return (SolidColorBrush)_props[number].GetValue(null, null);
		}
	}
}

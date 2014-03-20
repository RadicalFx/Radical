using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace Topics.Radical.Windows.Controls.Themes
{
	public partial class generic : ResourceDictionary
	{
		private void PART_Grip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 1)
			{
				(sender as FrameworkElement).CaptureMouse();
				Resizer.StartResizeCommand.Execute(sender as FrameworkElement, sender as FrameworkElement);
				e.Handled = true;
			}
		}

		private void PART_Grip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			FrameworkElement resizeGrip = sender as FrameworkElement;
			Debug.Assert(resizeGrip != null);

			if (resizeGrip.IsMouseCaptured)
			{
				Resizer.EndResizeCommand.Execute(null, sender as FrameworkElement);
				resizeGrip.ReleaseMouseCapture();
				e.Handled = true;
			}
		}

		private void PART_Grip_MouseMove(object sender, MouseEventArgs e)
		{
			if ((sender as FrameworkElement).IsMouseCaptured)
			{
				Resizer.UpdateSizeCommand.Execute(null, sender as FrameworkElement);
				e.Handled = true;
			}
		}

		private void PART_Grip_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Resizer.AutoSizeCommand.Execute(null, sender as FrameworkElement);
				e.Handled = true;
			}
		}
	}

	public sealed class GripAlignmentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Orientation orientation = (Orientation) parameter;
			ResizeDirection resizeDirection = (ResizeDirection) value;

			switch (orientation)
			{
				case Orientation.Horizontal:
					if (resizeDirection == ResizeDirection.NorthEast || resizeDirection == ResizeDirection.SouthEast)
					{
						return HorizontalAlignment.Right;
					}
					else
					{
						return HorizontalAlignment.Left;
					}
				case Orientation.Vertical:
					if (resizeDirection == ResizeDirection.NorthEast || resizeDirection == ResizeDirection.NorthWest)
					{
						return VerticalAlignment.Top;
					}
					else
					{
						return VerticalAlignment.Bottom;
					}
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}

	public sealed class GripCursorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			ResizeDirection resizeDirection = (ResizeDirection) value;

			switch (resizeDirection)
			{
				case ResizeDirection.NorthEast:
				case ResizeDirection.SouthWest:
					return Cursors.SizeNESW;
				case ResizeDirection.NorthWest:
				case ResizeDirection.SouthEast:
					return Cursors.SizeNWSE;
			}

			return DependencyProperty.UnsetValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}

	public sealed class GripRotationConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			ResizeDirection resizeDirection = (ResizeDirection) value;

			switch (resizeDirection)
			{
				case ResizeDirection.SouthWest:
					return 90;
				case ResizeDirection.NorthWest:
					return 180;
				case ResizeDirection.NorthEast:
					return 270;
			}

			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}

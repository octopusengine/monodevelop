﻿using MonoDevelop.Components.MainToolbar;
using MonoDevelop.Ide;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowsPlatform.MainToolbar
{
	/// <summary>
	/// Interaction logic for RunButton.xaml
	/// </summary>
	public partial class IconButtonControl : UserControl, INotifyPropertyChanged
	{
		ImageSource currentImage;
		public ImageSource CurrentImage
		{
			get { return currentImage; }
			set { currentImage = value; RaisePropertyChanged (); }
		}

		public IconButtonControl (ImageSource image)
		{
			InitializeComponent ();

			DataContext = this;
			IsEnabled = false;
			CurrentImage = image;
		}

		void OnClick (object sender, RoutedEventArgs args)
		{
			if (Click != null)
				Click (sender, args);
		}

		void RaisePropertyChanged ([CallerMemberName] string propName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged (this, new PropertyChangedEventArgs (propName));
		}

		public event RoutedEventHandler Click;
		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class RunButtonControl : IconButtonControl
	{
		RunButtonControl (OperationIcon icon) : base(GetIcon(icon))
		{
			this.icon = icon;
		}
		public RunButtonControl () : this (OperationIcon.Run)
		{
		}

		OperationIcon icon;
		public OperationIcon Icon
		{
			get { return icon; }
			set
			{
				if (value == icon)
					return;
				icon = value;
				CurrentImage = GetIcon (icon);
			}
		}

		static ImageSource GetIcon (OperationIcon icon)
		{
			string img;
			switch (icon) {
				case OperationIcon.Stop:
					img = "ico-stop-normal-32.png";
					break;
				case OperationIcon.Run:
					img = "ico-execute-normal-32.png";
					break;
				case OperationIcon.Build:
					img = "ico-build-normal-32.png";
					break;
				default:
					throw new InvalidOperationException ();
			}

			return (ImageSource)MonoDevelop.Platform.WindowsPlatform.WPFToolkit.GetNativeImage (Xwt.Drawing.Image.FromResource (typeof (RoundButton), img));
		}
	}

	public class ButtonBarButton : IconButtonControl, IDisposable
	{
		IButtonBarButton button;
		public ButtonBarButton (ImageSource image, IButtonBarButton button) : base (image)
		{
			this.button = button;

			Margin = new Thickness (1, 0, 1, 0);
			VerticalContentAlignment = VerticalAlignment.Center;
			ToolTip = button.Tooltip;
			IsEnabled = button.Enabled;
			Visibility = button.Visible ? Visibility.Visible : Visibility.Collapsed;

			button.EnabledChanged += OnButtonEnabledChanged;
			button.VisibleChanged += OnButtonVisibleChanged;
			button.TooltipChanged += OnButtonTooltipChanged;
			button.ImageChanged += OnButtonImageChanged;

			Click += OnButtonClicked;
		}

		public void Dispose ()
		{
			button.EnabledChanged -= OnButtonEnabledChanged;
			button.VisibleChanged -= OnButtonVisibleChanged;
			button.TooltipChanged -= OnButtonTooltipChanged;
			button.ImageChanged -= OnButtonImageChanged;
		}

		void OnButtonEnabledChanged (object sender, EventArgs args)
		{
			IsEnabled = button.Enabled;
		}

		void OnButtonVisibleChanged (object sender, EventArgs e)
		{
			Visibility = button.Visible ? Visibility.Visible : Visibility.Collapsed;
		}

		void OnButtonTooltipChanged (object sender, EventArgs args)
		{
			ToolTip = button.Tooltip;
		}

		void OnButtonImageChanged (object sender, EventArgs args)
		{
			CurrentImage = (ImageSource)MonoDevelop.Platform.WindowsPlatform.WPFToolkit.GetNativeImage (ImageService.GetIcon (MonoDevelop.Ide.Gui.Stock.Add));
        }

		void OnButtonClicked (object sender, RoutedEventArgs args)
		{
			button.NotifyPushed ();
		}
	}
}
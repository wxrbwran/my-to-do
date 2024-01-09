﻿using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Media;

namespace MyToDO.ViewModels
{
	public class SkinViewModel : BindableBase
	{
		private bool _isDarkTheme;
		public bool IsDarkTheme
		{
			get => _isDarkTheme;
			set
			{
				if (SetProperty(ref _isDarkTheme, value))
				{
					ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
				}
			}
		}

		public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

		public DelegateCommand<object> ChangeHueCommand { get; private set; }

		private readonly PaletteHelper paletteHelper = new PaletteHelper();

		public SkinViewModel()
		{
			ChangeHueCommand = new DelegateCommand<object>(ChangeHue);
		}

		private static void ModifyTheme(Action<ITheme> modificationAction)
		{
			var paletteHelper = new PaletteHelper();
			ITheme theme = paletteHelper.GetTheme();
			modificationAction?.Invoke(theme);
			paletteHelper.SetTheme(theme);
		}

		private void ChangeHue(object obj)
		{
			var hue = (Color)obj;
			ITheme theme = paletteHelper.GetTheme();
			theme.PrimaryLight = new ColorPair(hue.Lighten());
			theme.PrimaryMid = new ColorPair(hue);
			theme.PrimaryDark = new ColorPair(hue.Darken());
			paletteHelper.SetTheme(theme);
		}

	}
}

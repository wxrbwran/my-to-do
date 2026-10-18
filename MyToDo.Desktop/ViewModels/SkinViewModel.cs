using System.Collections.Generic;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SukiUI;
using SukiUI.Models;

namespace MyToDo.Desktop.ViewModels
{
    public partial class SkinViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isDarkTheme;

        [ObservableProperty]
        private IEnumerable<SukiColorTheme> _colorThemes;

        public SkinViewModel()
        {
            var suki = SukiTheme.GetInstance();
            ColorThemes = suki.ColorThemes;
            IsDarkTheme = suki.ActiveBaseTheme == ThemeVariant.Dark;
        }

        partial void OnIsDarkThemeChanged(bool value)
        {
            SukiTheme.GetInstance().ChangeBaseTheme(value ? ThemeVariant.Dark : ThemeVariant.Light);
        }

        [RelayCommand]
        private void ChangeColor(SukiColorTheme theme)
        {
            if (theme != null)
            {
                SukiTheme.GetInstance().ChangeColorTheme(theme);
            }
        }
    }
}

using System.Collections.Generic;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Material.Colors;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace MyToDo.Desktop.ViewModels
{
    public class ColorOption
    {
        public string Name { get; set; } = string.Empty;
        public string HexColor { get; set; } = "#1E88E5";
        public PrimaryColor ColorEnum { get; set; }
    }

    public partial class SkinViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isDarkTheme;

        [ObservableProperty]
        private List<ColorOption> _colorOptions = new();

        public SkinViewModel()
        {
            ColorOptions = new List<ColorOption>
            {
                new() { Name = "深海蓝", HexColor = "#1E88E5", ColorEnum = PrimaryColor.Blue },
                new() { Name = "宝石青", HexColor = "#00897B", ColorEnum = PrimaryColor.Teal },
                new() { Name = "罗兰紫", HexColor = "#8E24AA", ColorEnum = PrimaryColor.Purple },
                new() { Name = "翡翠绿", HexColor = "#43A047", ColorEnum = PrimaryColor.Green },
                new() { Name = "琥珀橙", HexColor = "#FB8C00", ColorEnum = PrimaryColor.Orange },
                new() { Name = "热情红", HexColor = "#E53935", ColorEnum = PrimaryColor.Red },
                new() { Name = "深靛青", HexColor = "#3949AB", ColorEnum = PrimaryColor.Indigo }
            };
        }

        partial void OnIsDarkThemeChanged(bool value)
        {
            var theme = Application.Current?.LocateMaterialTheme<MaterialTheme>();
            if (theme != null)
            {
                theme.BaseTheme = value ? BaseThemeMode.Dark : BaseThemeMode.Light;
            }
        }

        [RelayCommand]
        private void ChangePrimaryColor(ColorOption option)
        {
            if (option == null) return;
            var theme = Application.Current?.LocateMaterialTheme<MaterialTheme>();
            if (theme != null)
            {
                theme.PrimaryColor = option.ColorEnum;
            }
        }
    }
}

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyToDO.Views
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      btnMin.Click += (s, e) => {
        this.WindowState = WindowState.Minimized;
      }; 
      btnMax.Click += (s, e) => HandleMinMaxWindow(); 
      btnClose.Click += (s, e) => {
        this.Close();
      };
      colorZone.MouseMove += (s, e) =>
      {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
          this.DragMove();
        }
      };
      colorZone.MouseDoubleClick += (s, e) => HandleMinMaxWindow();
      memuBars.SelectionChanged += (s, e) =>
      {
        mainWindowDrawerHost.IsLeftDrawerOpen = false;
      };

    }
    public void HandleMinMaxWindow()
    {
      if (this.WindowState == WindowState.Maximized)
      {
        this.WindowState = WindowState.Normal;
      }
      else
      {
        this.WindowState = WindowState.Maximized;

      }
    }

  }
}
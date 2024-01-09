using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace MyToDO.Views
{
	/// <summary>
	/// AboutView.xaml 的交互逻辑
	/// </summary>
	public partial class AboutView : UserControl
	{
		public AboutView()
		{
			InitializeComponent();
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo("https://github.com/wxrbwran")
				{
					UseShellExecute = true,
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

	}
}

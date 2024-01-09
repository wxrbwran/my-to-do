using MyToDO.Extensions;
using Prism.Events;
using System.Windows;
using System.Windows.Input;

namespace MyToDO.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// private readonly IEventAggregator aggregator;

		public MainWindow(IEventAggregator aggregator)
		{
			InitializeComponent();
			btnMin.Click += (s, e) =>
			{
				this.WindowState = WindowState.Minimized;
			};
			btnMax.Click += (s, e) => HandleMinMaxWindow();
			btnClose.Click += (s, e) =>
			{
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
			this.Loaded += (s, e) => { };

			// 注册等待消息窗口
			aggregator.Register(arg =>
			{
				MainWindowDialogHost.IsOpen = arg.IsOpen;
				if (arg.IsOpen)
				{
					MainWindowDialogHost.DialogContent = new ProgressView();

				}
			});
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
using MyToDO.Common;
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
		private readonly IDialogHostService dialogHostService;

		// private readonly IEventAggregator aggregator;

		public MainWindow(IEventAggregator aggregator, IDialogHostService dialogHostService)
		{
			InitializeComponent();
			btnMin.Click += (s, e) =>
			{
				this.WindowState = WindowState.Minimized;
			};
			btnMax.Click += (s, e) => HandleMinMaxWindow();
			btnClose.Click += async (s, e) =>
			{
				var result = await dialogHostService.Question("确定退出吗", "点击确认退出系统");
				if (result.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
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

			// 注册等待消息窗口
			aggregator.RegisterUploading(arg =>
			{
				MainWindowDialogHost.IsOpen = arg.IsOpen;
				if (arg.IsOpen)
				{
					MainWindowDialogHost.DialogContent = new ProgressView();
				}
			});
			// 注册消息事件
			aggregator.RegisterMessage(messageModel =>
			{
				mainWindowSnackBar.MessageQueue.Enqueue(messageModel.Message);
			}, "MainWindow");
			this.dialogHostService = dialogHostService;
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
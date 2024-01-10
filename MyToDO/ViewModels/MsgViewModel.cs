using MaterialDesignThemes.Wpf;
using MyToDO.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;


namespace MyToDO.ViewModels
{
  public class MsgViewModel: BindableBase, IDialogHostAware
	{
		public string DialogHostName { get; set; } = "Root";
		public DelegateCommand SaveCommand { get; set; }
		public DelegateCommand CancelCommand { get; set; }

		public MsgViewModel()
		{
			SaveCommand = new DelegateCommand(Save);
			CancelCommand = new DelegateCommand(Cancel);
		}
		#region 属性
		private string title;

		public string Title
		{
			get { return title; }
			set { title = value; RaisePropertyChanged(); }
		}

		private string content;

		public string Content
		{
			get { return content; }
			set { content = value; RaisePropertyChanged(); }
		}

		#endregion 属性

		private void Cancel()
		{
			DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
		}

		private void Save()
		{
			DialogParameters parameter = new DialogParameters();
			DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameter));
		}

		public void OnDialogOpend(IDialogParameters parameters)
		{
			string t = parameters.GetValue<string>("Title");
			string c = parameters.GetValue<string>("Content");
			if (t != null)
			{
				Title = t;
			}
			if (c != null)
			{
				Content = c;
			}
		}
	}
}

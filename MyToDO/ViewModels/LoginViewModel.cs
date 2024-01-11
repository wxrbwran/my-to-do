using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyToDO.ViewModels
{
	public class LoginViewModel : BindableBase, IDialogAware
	{
		public string Title { get; set; } = "ToDo";

		public event Action<IDialogResult> RequestClose;

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{
		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
		}
	}
}

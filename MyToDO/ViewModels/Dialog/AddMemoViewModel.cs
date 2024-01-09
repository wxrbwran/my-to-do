using MaterialDesignThemes.Wpf;
using MyToDO.Common;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.ViewModels.Dialog
{
	public class AddMemoViewModel : IDialogHostAware
	{
		public string DialogHostName { get ; set ; }
		public DelegateCommand SaveCommand { get; set; }
		public DelegateCommand CancelCommand { get; set; }

		public AddMemoViewModel() 
		{
			SaveCommand = new DelegateCommand(Save);
			CancelCommand = new DelegateCommand(Cancel);
		}

		private void Cancel()
		{
			DialogHost.Close(DialogHostName);
		}

		private void Save()
		{
			DialogParameters parameter = new DialogParameters();
			DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameter));

		}

		public void OnDialogOpend(IDialogParameters parameters)
		{
		}
	}
}

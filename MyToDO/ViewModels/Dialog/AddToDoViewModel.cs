using MaterialDesignThemes.Wpf;
using MyToDo.Shared.Dtos;
using MyToDO.Common;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;


namespace MyToDO.ViewModels.Dialog
{
	public class AddToDoViewModel :BindableBase, IDialogHostAware
	{
		public string DialogHostName { get; set; }
		public DelegateCommand SaveCommand { get; set; }
		public DelegateCommand CancelCommand { get; set; }

		public AddToDoViewModel()
		{
			SaveCommand = new DelegateCommand(Save);
			CancelCommand = new DelegateCommand(Cancel);
		}

		#region 属性
		private ToDoDto model;
		public ToDoDto Model
		{
			get { return model; }
			set { model = value; RaisePropertyChanged(); }
		}
		#endregion 属性

		private void Cancel()
		{
			DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.Cancel));
		}

		private void Save()
		{
			if (model == null || string.IsNullOrWhiteSpace(model.Title)) { return; }
			if (DialogHost.IsDialogOpen(DialogHostName))
			{
				DialogParameters parameter = new DialogParameters();
				parameter.Add("Value", Model);
				DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, parameter));
			}

		}

		public void OnDialogOpend(IDialogParameters parameters)
		{
			if (parameters.ContainsKey("Value"))
			{
				Model = parameters.GetValue<ToDoDto>("Value");
			}
			else
			{
				Model = new ToDoDto();
			}
		}
	}
}

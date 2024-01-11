using MyToDO.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using MyToDo.Shared;
using MyToDO.Common.Models;
using Prism.Events;
using MyToDO.Extensions;
using System.Windows;
using MyToDO.Common;

namespace MyToDO.ViewModels
{
	public class LoginViewModel : BindableBase, IDialogAware
	{
		public DelegateCommand<string> ExecuteCommand { get; set; }
		public event Action<IDialogResult> RequestClose;
		private readonly IAuthService service;
		private readonly IEventAggregator aggregator;

		public LoginViewModel(IAuthService service, IEventAggregator aggregator)
    {
			UserDto = new RegisterUserDto();
			ExecuteCommand = new DelegateCommand<string>(Execute);
			this.service = service;
			this.aggregator = aggregator;
		}

		#region 属性
		public string Title { get; set; } = "ToDo";

		private string account;
		public string Account
		{
			get { return account; }
			set { account = value; RaisePropertyChanged(); }
		}

		private string password;
		public string Password
		{
			get { return password; }
			set { password = value; RaisePropertyChanged(); }
		}

		private int selectedIndex;
		public int SelectedIndex
		{
			get { return selectedIndex; }
			set { selectedIndex = value; RaisePropertyChanged(); }
		}

		private RegisterUserDto userDto;
		public RegisterUserDto UserDto
		{
			get { return userDto; }
			set { userDto = value; RaisePropertyChanged(); }
		}


		#endregion 属性

		private void Execute(string command)
		{
			switch (command)
			{
				case "Login":
					Login();
					break;
				case "LogOut":
					LogOut();
					break;
				case "ToggleLoginSlide":
					SelectedIndex = 0;
					break;
				case "ToggleRegisterSlide":
					SelectedIndex = 1;
					break;
				case "Register":
					Register();
					break;
			}
		}
		
		private async void Login()
		{
			if(string.IsNullOrWhiteSpace(Account) || 
				string.IsNullOrWhiteSpace(Password)) 
			{
				return;
			}
			var apiResponse = await service.LoginAsync(new MyToDo.Shared.Dtos.UserDto() { Account = Account, Password = Password });
			if(apiResponse.Status)
			{
				AppSession.Username = apiResponse.Result.Username!;
				RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
			} else
			{
				aggregator.SendMessage(apiResponse.Message, "LoginView");
			}
		}
		private void LogOut()
		{
			RequestClose?.Invoke(new DialogResult(ButtonResult.No));
		}

		private async void Register()
		{
			if (string.IsNullOrWhiteSpace(UserDto.Account) || string.IsNullOrWhiteSpace(UserDto.Username) ||
				string.IsNullOrWhiteSpace(UserDto.Password) || string.IsNullOrWhiteSpace(UserDto.ConfirmPassword))
			{
				aggregator.SendMessage("请完善注册信息！", "LoginView");
				return;
			}
			if (!string.IsNullOrWhiteSpace(UserDto.Password).Equals(string.IsNullOrWhiteSpace(UserDto.ConfirmPassword)))
			{
				aggregator.SendMessage("密码不一致！", "LoginView");
				return;
			}
			ApiResponse resp = await service.RegisterAsync(new MyToDo.Shared.Dtos.UserDto() { 
				Account = UserDto.Account,
				Username = UserDto.Username,
				Password = UserDto.Password,
			});
			if (resp.Status && resp.Result != null)
			{
				// 注册成功
				aggregator.SendMessage("注册成功！", "LoginView");
				SelectedIndex = 0;
			} else
			{
				// 注册失败
				aggregator.SendMessage("注册失败！", "LoginView");
			}
		}

		public bool CanCloseDialog()
		{
			return true;
		}

		public void OnDialogClosed()
		{
			LogOut();
		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
		}
	}
}

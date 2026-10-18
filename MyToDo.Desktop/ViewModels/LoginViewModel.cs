using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyToDo.Desktop.Models;
using MyToDo.Desktop.Services;
using MyToDo.Shared.Dtos;

namespace MyToDo.Desktop.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly Action _loginSuccessCallback;

        [ObservableProperty]
        private string _account = "admin";

        [ObservableProperty]
        private string _username = "小然";

        [ObservableProperty]
        private string _password = "123";

        [ObservableProperty]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private bool _isRegisterMode;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public LoginViewModel(IAuthService authService, Action loginSuccessCallback)
        {
            _authService = authService;
            _loginSuccessCallback = loginSuccessCallback;
        }

        [RelayCommand]
        private void ToggleMode()
        {
            IsRegisterMode = !IsRegisterMode;
            ErrorMessage = string.Empty;
        }

        [RelayCommand]
        private async Task SubmitAsync()
        {
            ErrorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(Account) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "账号或密码不能为空！";
                return;
            }

            IsLoading = true;
            try
            {
                if (IsRegisterMode)
                {
                    if (Password != ConfirmPassword)
                    {
                        ErrorMessage = "两次输入的密码不一致！";
                        return;
                    }

                    var user = new UserDto
                    {
                        Account = Account,
                        Username = string.IsNullOrWhiteSpace(Username) ? Account : Username,
                        Password = Password
                    };

                    var res = await _authService.RegisterAsync(user);
                    if (res.Status)
                    {
                        IsRegisterMode = false;
                        ErrorMessage = "注册成功，请使用新账号登录！";
                    }
                    else
                    {
                        ErrorMessage = res.Message ?? "注册失败";
                    }
                }
                else
                {
                    var user = new UserDto { Account = Account, Password = Password };
                    var res = await _authService.LoginAsync(user);
                    if (res.Status && res.Result != null)
                    {
                        AppSession.Account = res.Result.Account;
                        AppSession.Username = res.Result.Username;
                        AppSession.IsLoggedIn = true;
                        _loginSuccessCallback?.Invoke();
                    }
                    else
                    {
                        // 离线/模拟登录兜底
                        AppSession.Account = Account;
                        AppSession.Username = string.IsNullOrWhiteSpace(Username) ? Account : Username;
                        AppSession.IsLoggedIn = true;
                        _loginSuccessCallback?.Invoke();
                    }
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}

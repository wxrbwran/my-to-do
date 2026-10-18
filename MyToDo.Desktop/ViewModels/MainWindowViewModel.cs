using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyToDo.Desktop.Models;
using MyToDo.Desktop.Services;
using MyToDo.Shared.Dtos;

namespace MyToDo.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IToDoService _toDoService;
        private readonly IMemoService _memoService;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<MenuBar> _menuBars = new();

        [ObservableProperty]
        private MenuBar? _selectedMenu;

        [ObservableProperty]
        private ViewModelBase? _currentView;

        [ObservableProperty]
        private string _username = "小然";

        // 弹窗状态
        [ObservableProperty]
        private bool _isDialogOpen;

        [ObservableProperty]
        private string _dialogTitle = string.Empty;

        [ObservableProperty]
        private ViewModelBase? _dialogContent;

        // 缓存的各个页面 VM
        public IndexViewModel IndexVm { get; }
        public ToDoViewModel ToDoVm { get; }
        public MemoViewModel MemoVm { get; }
        public SkinViewModel SkinVm { get; }
        public LoginViewModel LoginVm { get; }

        public MainWindowViewModel(IToDoService toDoService, IMemoService memoService, IAuthService authService)
        {
            _toDoService = toDoService;
            _memoService = memoService;
            _authService = authService;

            IndexVm = new IndexViewModel(toDoService, memoService, NavigateTo, OpenDialog);
            ToDoVm = new ToDoViewModel(toDoService, OpenDialog);
            MemoVm = new MemoViewModel(memoService, OpenDialog);
            SkinVm = new SkinViewModel();
            LoginVm = new LoginViewModel(authService, OnLoginSuccess);

            InitMenuBars();

            // 默认展示登录页面，若已登录则展示首页
            if (AppSession.IsLoggedIn)
            {
                OnLoginSuccess();
            }
            else
            {
                CurrentView = LoginVm;
            }
        }

        private void InitMenuBars()
        {
            MenuBars.Clear();
            MenuBars.Add(new MenuBar { Index = 0, Icon = "Home", Title = "首页", TargetView = "IndexView" });
            MenuBars.Add(new MenuBar { Index = 1, Icon = "FormatListBulleted", Title = "待办事项", TargetView = "ToDoView" });
            MenuBars.Add(new MenuBar { Index = 2, Icon = "NotebookEditOutline", Title = "备忘录", TargetView = "MemoView" });
            MenuBars.Add(new MenuBar { Index = 3, Icon = "PaletteOutline", Title = "个性化", TargetView = "SkinView" });
            SelectedMenu = MenuBars[0];
        }

        public void NavigateTo(string targetView)
        {
            switch (targetView)
            {
                case "IndexView":
                    SelectedMenu = MenuBars[0];
                    CurrentView = IndexVm;
                    _ = IndexVm.RefreshDataAsync();
                    break;
                case "ToDoView":
                    SelectedMenu = MenuBars[1];
                    CurrentView = ToDoVm;
                    _ = ToDoVm.RefreshDataAsync();
                    break;
                case "MemoView":
                    SelectedMenu = MenuBars[2];
                    CurrentView = MemoVm;
                    _ = MemoVm.RefreshDataAsync();
                    break;
                case "SkinView":
                    SelectedMenu = MenuBars[3];
                    CurrentView = SkinVm;
                    break;
            }
        }

        partial void OnSelectedMenuChanged(MenuBar? value)
        {
            if (value != null)
            {
                NavigateTo(value.TargetView);
            }
        }

        public void OnLoginSuccess()
        {
            Username = AppSession.Username;
            NavigateTo("IndexView");
        }

        [RelayCommand]
        private void Logout()
        {
            AppSession.IsLoggedIn = false;
            CurrentView = LoginVm;
        }

        public void OpenDialog(string dialogType, object? parameter)
        {
            if (dialogType == "AddToDo")
            {
                DialogTitle = parameter == null ? "添加待办事项" : "编辑待办事项";
                DialogContent = new AddToDoDialogViewModel(parameter as ToDoDto, async (savedItem) =>
                {
                    if (savedItem.Id > 0)
                        await _toDoService.UpdateAsync(savedItem);
                    else
                        await _toDoService.AddAsync(savedItem);

                    IsDialogOpen = false;
                    if (CurrentView is IndexViewModel idx) _ = idx.RefreshDataAsync();
                    if (CurrentView is ToDoViewModel td) _ = td.RefreshDataAsync();
                }, () => IsDialogOpen = false);
                IsDialogOpen = true;
            }
            else if (dialogType == "AddMemo")
            {
                DialogTitle = parameter == null ? "添加备忘录" : "编辑备忘录";
                DialogContent = new AddMemoDialogViewModel(parameter as MemoDto, async (savedItem) =>
                {
                    if (savedItem.Id > 0)
                        await _memoService.UpdateAsync(savedItem);
                    else
                        await _memoService.AddAsync(savedItem);

                    IsDialogOpen = false;
                    if (CurrentView is IndexViewModel idx) _ = idx.RefreshDataAsync();
                    if (CurrentView is MemoViewModel mm) _ = mm.RefreshDataAsync();
                }, () => IsDialogOpen = false);
                IsDialogOpen = true;
            }
        }

        [RelayCommand]
        private void CloseDialog()
        {
            IsDialogOpen = false;
        }
    }

    public partial class AddToDoDialogViewModel : ViewModelBase
    {
        private readonly Action<ToDoDto> _saveCallback;
        private readonly Action _cancelCallback;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _content = string.Empty;

        [ObservableProperty]
        private int _status;

        public AddToDoDialogViewModel(ToDoDto? existing, Action<ToDoDto> saveCallback, Action cancelCallback)
        {
            _saveCallback = saveCallback;
            _cancelCallback = cancelCallback;

            if (existing != null)
            {
                Id = existing.Id;
                Title = existing.Title ?? string.Empty;
                Content = existing.Content ?? string.Empty;
                Status = existing.Status;
            }
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title)) return;
            _saveCallback?.Invoke(new ToDoDto
            {
                Id = Id,
                Title = Title,
                Content = Content,
                Status = Status
            });
        }

        [RelayCommand]
        private void Cancel()
        {
            _cancelCallback?.Invoke();
        }
    }

    public partial class AddMemoDialogViewModel : ViewModelBase
    {
        private readonly Action<MemoDto> _saveCallback;
        private readonly Action _cancelCallback;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _content = string.Empty;

        public AddMemoDialogViewModel(MemoDto? existing, Action<MemoDto> saveCallback, Action cancelCallback)
        {
            _saveCallback = saveCallback;
            _cancelCallback = cancelCallback;

            if (existing != null)
            {
                Id = existing.Id;
                Title = existing.Title ?? string.Empty;
                Content = existing.Content ?? string.Empty;
            }
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title)) return;
            _saveCallback?.Invoke(new MemoDto
            {
                Id = Id,
                Title = Title,
                Content = Content
            });
        }

        [RelayCommand]
        private void Cancel()
        {
            _cancelCallback?.Invoke();
        }
    }
}

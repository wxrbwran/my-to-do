using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyToDo.Desktop.Services;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;

namespace MyToDo.Desktop.ViewModels
{
    public partial class ToDoViewModel : ViewModelBase
    {
        private readonly IToDoService _toDoService;
        private readonly Action<string, object?> _openDialogCallback;

        [ObservableProperty]
        private ObservableCollection<ToDoDto> _todoList = new();

        [ObservableProperty]
        private string _searchKey = string.Empty;

        [ObservableProperty]
        private int _selectedFilterIndex = 0; // 0=全部, 1=待办, 2=已完成

        [ObservableProperty]
        private bool _isLoading;

        public ToDoViewModel(IToDoService toDoService, Action<string, object?> openDialogCallback)
        {
            _toDoService = toDoService;
            _openDialogCallback = openDialogCallback;
        }

        partial void OnSelectedFilterIndexChanged(int value)
        {
            _ = RefreshDataAsync();
        }

        public async Task RefreshDataAsync()
        {
            IsLoading = true;
            try
            {
                int? status = SelectedFilterIndex switch
                {
                    1 => 0, // 待办
                    2 => 1, // 已完成
                    _ => null
                };

                var param = new ToDoQueryParameter
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = SearchKey,
                    Status = status
                };

                var res = await _toDoService.GetAllFilterAsync(param);
                if (res.Status && res.Result?.Items != null)
                {
                    TodoList = new ObservableCollection<ToDoDto>(res.Result.Items);
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            await RefreshDataAsync();
        }

        [RelayCommand]
        private void AddToDo()
        {
            _openDialogCallback?.Invoke("AddToDo", null);
        }

        [RelayCommand]
        private void EditToDo(ToDoDto item)
        {
            if (item != null)
                _openDialogCallback?.Invoke("AddToDo", item);
        }

        [RelayCommand]
        private async Task DeleteToDoAsync(ToDoDto item)
        {
            if (item == null) return;
            await _toDoService.DeleteAsync(item.Id);
            TodoList.Remove(item);
        }

        [RelayCommand]
        private async Task ToggleToDoStatusAsync(ToDoDto item)
        {
            if (item == null) return;
            item.Status = item.Status == 0 ? 1 : 0;
            await _toDoService.UpdateAsync(item);
            await RefreshDataAsync();
        }
    }
}

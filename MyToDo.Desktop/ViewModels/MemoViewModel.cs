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
    public partial class MemoViewModel : ViewModelBase
    {
        private readonly IMemoService _memoService;
        private readonly Action<string, object?> _openDialogCallback;

        [ObservableProperty]
        private ObservableCollection<MemoDto> _memoList = new();

        [ObservableProperty]
        private string _searchKey = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public MemoViewModel(IMemoService memoService, Action<string, object?> openDialogCallback)
        {
            _memoService = memoService;
            _openDialogCallback = openDialogCallback;
        }

        public async Task RefreshDataAsync()
        {
            IsLoading = true;
            try
            {
                var param = new QueryParameter
                {
                    PageIndex = 0,
                    PageSize = 100,
                    Search = SearchKey
                };

                var res = await _memoService.GetAllAsync(param);
                if (res.Status && res.Result?.Items != null)
                {
                    MemoList = new ObservableCollection<MemoDto>(res.Result.Items);
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
        private void AddMemo()
        {
            _openDialogCallback?.Invoke("AddMemo", null);
        }

        [RelayCommand]
        private void EditMemo(MemoDto item)
        {
            if (item != null)
                _openDialogCallback?.Invoke("AddMemo", item);
        }

        [RelayCommand]
        private async Task DeleteMemoAsync(MemoDto item)
        {
            if (item == null) return;
            await _memoService.DeleteAsync(item.Id);
            MemoList.Remove(item);
        }
    }
}

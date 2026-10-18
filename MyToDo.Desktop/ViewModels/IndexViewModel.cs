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
    public partial class IndexViewModel : ViewModelBase
    {
        private readonly IToDoService _toDoService;
        private readonly IMemoService _memoService;
        private readonly Action<string> _navigationCallback;
        private readonly Action<string, object?> _openDialogCallback;

        [ObservableProperty]
        private string _welcome = string.Empty;

        [ObservableProperty]
        private SummaryDto _summary = new();

        [ObservableProperty]
        private ObservableCollection<IndexTaskBar> _taskBars = new();

        [ObservableProperty]
        private ObservableCollection<ToDoDto> _todoList = new();

        [ObservableProperty]
        private ObservableCollection<MemoDto> _memoList = new();

        public IndexViewModel(IToDoService toDoService, IMemoService memoService, Action<string> navigationCallback, Action<string, object?> openDialogCallback)
        {
            _toDoService = toDoService;
            _memoService = memoService;
            _navigationCallback = navigationCallback;
            _openDialogCallback = openDialogCallback;

            UpdateWelcome();
            InitTaskBars();
        }

        public void UpdateWelcome()
        {
            Welcome = $"你好，{AppSession.Username}！今天是 {DateTime.Now:yyyy年MM月dd日 dddd}。";
        }

        private void InitTaskBars()
        {
            TaskBars.Clear();
            TaskBars.Add(new IndexTaskBar { Icon = "ClockFast", Title = "待办汇总", Content = "0", Color = "#1E88E5", Target = "ToDoView", MainWindowMenuIndex = 1 });
            TaskBars.Add(new IndexTaskBar { Icon = "CheckCircleOutline", Title = "已完成", Content = "0", Color = "#43A047", Target = "ToDoView", MainWindowMenuIndex = 1 });
            TaskBars.Add(new IndexTaskBar { Icon = "ChartLine", Title = "完成比例", Content = "0%", Color = "#0288D1", Target = "ToDoView", MainWindowMenuIndex = 1 });
            TaskBars.Add(new IndexTaskBar { Icon = "NoteTextOutline", Title = "备忘录", Content = "0", Color = "#FB8C00", Target = "MemoView", MainWindowMenuIndex = 2 });
        }

        public async Task RefreshDataAsync()
        {
            UpdateWelcome();
            var res = await _toDoService.GetSummaryAsync();
            if (res.Status && res.Result != null)
            {
                Summary = res.Result;
                TaskBars[0].Content = Summary.TodoCount.ToString();
                TaskBars[1].Content = Summary.CompletedToDoCount.ToString();
                TaskBars[2].Content = Summary.CompletedToDoRatio;
                TaskBars[3].Content = Summary.MemoCount.ToString();

                TodoList = Summary.ToDoList != null ? new ObservableCollection<ToDoDto>(Summary.ToDoList) : new ObservableCollection<ToDoDto>();
                MemoList = Summary.MemoList != null ? new ObservableCollection<MemoDto>(Summary.MemoList) : new ObservableCollection<MemoDto>();
            }
            else
            {
                // 演示兜底数据（API未启动或连接异常时）
                if (TodoList.Count == 0)
                {
                    TodoList = new ObservableCollection<ToDoDto>
                    {
                        new ToDoDto { Id = 1, Title = "欢迎使用跨平台 MyToDo", Content = "已升级至 .NET 10 + Avalonia + Material Design", Status = 0 },
                        new ToDoDto { Id = 2, Title = "体验深色模式与主题切换", Content = "在设置中可自由切换 Light/Dark 与调色板主色", Status = 1 }
                    };
                    MemoList = new ObservableCollection<MemoDto>
                    {
                        new MemoDto { Id = 1, Title = "项目备忘", Content = "支持在 macOS, Windows, Linux 跨平台运行。" }
                    };
                    TaskBars[0].Content = TodoList.Count.ToString();
                    TaskBars[1].Content = "1";
                    TaskBars[2].Content = "50%";
                    TaskBars[3].Content = MemoList.Count.ToString();
                }
            }
        }

        [RelayCommand]
        private void TaskBarNavigate(IndexTaskBar bar)
        {
            if (bar != null && !string.IsNullOrWhiteSpace(bar.Target))
            {
                _navigationCallback?.Invoke(bar.Target);
            }
        }

        [RelayCommand]
        private void AddToDo()
        {
            _openDialogCallback?.Invoke("AddToDo", null);
        }

        [RelayCommand]
        private void AddMemo()
        {
            _openDialogCallback?.Invoke("AddMemo", null);
        }

        [RelayCommand]
        private void EditToDo(ToDoDto item)
        {
            if (item != null)
                _openDialogCallback?.Invoke("AddToDo", item);
        }

        [RelayCommand]
        private void EditMemo(MemoDto item)
        {
            if (item != null)
                _openDialogCallback?.Invoke("AddMemo", item);
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

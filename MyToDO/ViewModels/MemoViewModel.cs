using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDO.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace MyToDO.ViewModels
{
	public class MemoViewModel : NavigationViewModel
	{
		private readonly IMemoService service;
		public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
		public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
		public DelegateCommand<string> ExecuteCommand { get; private set; }

		public MemoViewModel(IMemoService service, IContainerProvider provider) : base(provider)
		{
			memoDtos = new ObservableCollection<MemoDto>();
			ExecuteCommand = new DelegateCommand<string>(Execute);
			this.service = service;
			SelectedCommand = new DelegateCommand<MemoDto>(HandleSelectMemoItem);
			DeleteCommand = new DelegateCommand<MemoDto>(HandleDeleteMemoItem);
		}

		#region 属性
		private ObservableCollection<MemoDto> memoDtos;

		public ObservableCollection<MemoDto> MemoDtos
		{
			get { return memoDtos; }
			set { memoDtos = value; RaisePropertyChanged(); }
		}

		private string isEmptyData;

		public string IsEmptyData
		{
			get { return isEmptyData; }
			set { isEmptyData = value; RaisePropertyChanged(); }
		}

		private string search;

		public string Search
		{
			get { return search; }
			set { search = value; RaisePropertyChanged(); }
		}

		private bool isRightDrawerOpen;

		public bool IsRightDrawerOpen
		{
			get { return isRightDrawerOpen; }
			set { isRightDrawerOpen = value; RaisePropertyChanged(); }
		}

		/// <summary>
		/// 编辑选中/新增时对象
		/// </summary>
		private MemoDto currentDto;
		public MemoDto CurrentDto
		{
			get { return currentDto; }
			set { currentDto = value; RaisePropertyChanged(); }
		}
		#endregion 属性

		#region 简单命令
		private void Execute(string command)
		{
			switch (command)
			{
				case "ShowAddMemo":
					ShowAddMemo(); break;
				case "SearchMemo":
					SearchMemo(); break;
				case "SaveEditMemo":
					SaveEditMemo(); break;
			}
		}

		private void SearchMemo()
		{
			GetDataAsync();
		}
		private void ShowAddMemo()
		{
			IsRightDrawerOpen = true;
			CurrentDto = new MemoDto();
		}

		private async void SaveEditMemo()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(currentDto.Title) || string.IsNullOrWhiteSpace(currentDto.Content))
				{
					return;
				}
				UpdateLoading(true);
				if (currentDto.Id > 0)
				// 编辑
				{
					var resp = await service.UpdateAsync(currentDto);
					if (resp.Status)
					{
						GetDataAsync();
					}
				}
				else
				{
					//新增
					var resp = await service.AddAsync(currentDto);
					if (resp.Status)
					{
						GetDataAsync();
					}
				}

			}
			catch (Exception ex)
			{

			}
			finally
			{
				UpdateLoading(false);
				IsRightDrawerOpen = false;
			}
		}

		#endregion 简单命令

		private async void HandleSelectMemoItem(MemoDto dto)
		{
			try
			{
				UpdateLoading(true);
				IsRightDrawerOpen = true;
				var resp = await service.GetFirstOfDefaultAsync(dto.Id);
				if (resp.Status)
				{
					CurrentDto = resp.Result;
					IsRightDrawerOpen = true;
				}

			}
			catch (Exception ex) { }
			finally { UpdateLoading(false); }
		}

		private async void HandleDeleteMemoItem(MemoDto dto)
		{
			try
			{
				await service.DeleteAsync(dto.Id);
				GetDataAsync();
			}
			catch (Exception ex) { }
		}

		public async void GetDataAsync()
		{
			UpdateLoading(true);
			var resp = await service.GetAllAsync(new QueryParameter()
			{
				PageIndex = 0,
				PageSize = 100,
				Search = Search,
			});
			if (resp.Status)
			{
				MemoDtos.Clear();
				foreach (var item in resp.Result.Items)
				{
					MemoDtos.Add(item);
				}
				IsEmptyData = (MemoDtos.Count == 0) ? "Visiable" : "Hidden";
			}
			UpdateLoading(false);
		}

		#region  导航进入
		public override void OnNavigatedTo(NavigationContext navigationContext)
		{
			base.OnNavigatedTo(navigationContext);
			GetDataAsync();
		}
		#endregion

	}
}

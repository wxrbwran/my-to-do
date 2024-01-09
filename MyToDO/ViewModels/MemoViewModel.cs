using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using MyToDO.Common.Models;
using MyToDO.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.ViewModels
{
  public class MemoViewModel: BindableBase
  {
		private readonly IMemoService service;

		public MemoViewModel(IMemoService service)
    {
      MemoDtos = new ObservableCollection<MemoDto>();
      ShowAddMemoCommand = new DelegateCommand(ShowAddMemo);
			this.service = service;
			HandleGetMemoLists();
		}

    private bool isRightDrawerOpen;

    public bool IsRightDrawerOpen
    {
      get { return isRightDrawerOpen; }
      set { isRightDrawerOpen = value; RaisePropertyChanged(); }
    }


    private void ShowAddMemo()
    {
      IsRightDrawerOpen = true;
    }

    public DelegateCommand ShowAddMemoCommand { get; private set; }

    private ObservableCollection<MemoDto> memoDtos;
		public ObservableCollection<MemoDto> MemoDtos
    {
      get { return memoDtos; }
      set { memoDtos = value; RaisePropertyChanged(); }
    }

		public async void HandleGetMemoLists()
		{
			var resp = await service.GetAllAsync(new QueryParameter()
			{
				PageIndex = 0,
				PageSize = 100,
			});
			if (resp.Status)
			{
				foreach (var item in resp.Result.Items)
				{
					memoDtos.Add(item);
				}
			}
		}
	}
}

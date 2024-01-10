using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
	public class SummaryDto : BaseDto
	{
		private int todoCount;
		public int TodoCount 
		{ 
			get => todoCount; 
			set { todoCount = value; OnPropertyChanged(); }
		}

		private int completedToDoCount;
		public int CompletedToDoCount
		{
			get => completedToDoCount;
			set { completedToDoCount = value; OnPropertyChanged(); }
		}

		private string completedToDoRatio = "";
		public string CompletedToDoRatio
		{
			get => completedToDoRatio;
			set { completedToDoRatio = value; OnPropertyChanged(); }
		}

		private int memoCount;
		public int MemoCount
		{
			get => memoCount;
			set { memoCount = value; OnPropertyChanged(); }
		}

		private ObservableCollection<ToDoDto> todoList;
		public ObservableCollection<ToDoDto> ToDoList 
		{
			get {  return todoList; }
			set { todoList = value; OnPropertyChanged(); }
		}

		private ObservableCollection<MemoDto> memoList;
		public ObservableCollection<MemoDto> MemoList
		{
			get { return memoList; }
			set { memoList = value; OnPropertyChanged(); }
		}

	}
}

using Prism.Mvvm;

namespace MyToDO.Common.Models
{
	/// <summary>
	/// 系统导航菜单实体类
	/// </summary>
	public class MenuBar : BindableBase
	{
		private int index;
		public int Index
		{
			get { return index; }
			set { index = value; }
		}

		private string icon;
		public string Icon
		{
			get { return icon; }
			set { icon = value; }
		}

		private string title;
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		private string nameSpace;
		public string NameSpace
		{
			get { return nameSpace; }
			set { nameSpace = value; }
		}

	}
}

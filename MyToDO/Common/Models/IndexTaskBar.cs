using Prism.Mvvm;

namespace MyToDO.Common.Models
{
	public class IndexTaskBar : BindableBase
	{
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

		private string content;

		public string Content
		{
			get { return content; }
			set { content = value; RaisePropertyChanged(); }
		}

		private string color;

		public string Color
		{
			get { return color; }
			set { color = value; }
		}

		private string target;

		public string Target
		{
			get { return target; }
			set { target = value; }
		}



	}
}

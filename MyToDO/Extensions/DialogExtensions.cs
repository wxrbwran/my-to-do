using MyToDO.Common;
using MyToDO.Events;
using Prism.Events;
using Prism.Services.Dialogs;

namespace MyToDO.Extensions
{
	public static class DialogExtensions
	{

		/// <summary>
		/// 询问窗口
		/// </summary>
		/// <param name="dialogHostService"></param>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <param name="dialogHostName"></param>
		/// <returns></returns>
		public static async Task<IDialogResult> Question(this IDialogHostService dialogHostService,
			string title, string content, string dialogHostName = "Root") 
		{
			DialogParameters parameter = new DialogParameters();
			parameter.Add("Title", title);
			parameter.Add("Content", content);
			parameter.Add("dialogHostName", dialogHostName);
			return await dialogHostService.ShowDialog("MsgView", parameter, dialogHostName);
		}

		/// <summary>
		/// 推送加载消息
		/// </summary>
		/// <param name="aggregator"></param>
		/// <param name="updateModal"></param>
		public static void UpdateLoading(this IEventAggregator aggregator, UpdateModal updateModal)
		{
			aggregator.GetEvent<UpdateLoadingEvents>().Publish(updateModal);
		}
		/// <summary>
		/// 注册等待消息
		/// </summary>
		/// <param name="aggregator"></param>
		/// <param name="action"></param>
		public static void Register(this IEventAggregator aggregator, Action<UpdateModal> action)
		{
			aggregator.GetEvent<UpdateLoadingEvents>().Subscribe(action);
		}
	}
}

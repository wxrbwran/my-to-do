using MyToDO.Events;
using Prism.Events;

namespace MyToDO.Extensions
{
	public static class DialogExtensions
	{
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

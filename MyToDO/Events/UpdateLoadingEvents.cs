using Prism.Events;

namespace MyToDO.Events
{
	public class UpdateModal
	{
		public bool IsOpen { get; set; }
	}
	public class UpdateLoadingEvents : PubSubEvent<UpdateModal>
	{

	}
}

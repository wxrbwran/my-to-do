using MyToDo.Shared.Dtos;

namespace MyToDO.Service
{
	public class MemoService : BaseService<MemoDto>, IMemoService
	{
		private readonly HttpRestClient client;

		public MemoService(HttpRestClient client) : base(client, "Memo")
		{
			this.client = client;
		}
	}
}

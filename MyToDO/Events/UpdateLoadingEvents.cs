using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Events
{
	public class UpdateModal 
	{
		public bool IsOpen { get; set; }
	}
	public class UpdateLoadingEvents:PubSubEvent<UpdateModal>
	{

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Common.Models
{
  public class BaseDto
  {
		private int id;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		private DateTime createdAt;

		public DateTime CreatedAt
		{
			get { return createdAt; }
			set { createdAt = value; }
		}

		private DateTime? updatedAt;

		public DateTime? UpdatedAt
		{
			get { return updatedAt; }
			set { updatedAt = value; }
		}

		private DateTime? deletedAt;

		public DateTime? DeletedAt
		{
			get { return deletedAt; }
			set { deletedAt = value; }
		}


	}
}

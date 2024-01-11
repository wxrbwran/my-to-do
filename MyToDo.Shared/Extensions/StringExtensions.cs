using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Extensions
{
	public static class StringExtensions
	{
		public static string GetMD5(this string value) 
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException(nameof(value));
			}
			var hash = MD5.Create().ComputeHash(Encoding.Default.GetBytes(value));

			return Convert.ToBase64String(hash);
		}
	}
}

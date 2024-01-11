using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDO.Common.Models
{
	public class RegisterUserDto: UserDto, INotifyPropertyChanged
	{
		private string confirmPassword;
		public string ConfirmPassword
		{
			get { return confirmPassword; }
			set { confirmPassword = value; OnPropertyChanged(); }
		}

	}
}

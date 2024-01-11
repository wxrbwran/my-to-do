﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
  public class UserDto: BaseDto
  {
    private string account;
    private string? username;
    private string? password;

    public string Account 
    { 
      get { return account; }
      set { account = value;  OnPropertyChanged(); }
    }

    public string? Username 
    { 
      get { return username; }
      set { username = value; OnPropertyChanged(); } 
    }

    public string Password
    {
      get { return password; }
      set { password = value; OnPropertyChanged(); }
    }

  }
}

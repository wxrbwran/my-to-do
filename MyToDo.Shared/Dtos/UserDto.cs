using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Dtos
{
  public class UserDto: BaseDto
  {
    public string Account { get; set; }

    public string Username { get; set; }

  }
}

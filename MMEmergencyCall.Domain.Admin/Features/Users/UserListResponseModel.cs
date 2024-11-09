using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMEmergencyCall.Databases.AppDbContextModels;

namespace MMEmergencyCall.Domain.Admin.Features.Users;

public class UserListResponseModel
{
    public List<User> UserList { get; set; } = new List<User>();
}
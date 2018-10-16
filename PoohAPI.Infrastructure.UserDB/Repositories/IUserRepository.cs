﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Infrastructure.UserDB.Models;

namespace PoohAPI.Infrastructure.UserDB.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<WPUser> GetAllUsers(string query);
        WPUser GetUser(string query);
        int RegisterUser(string query, Dictionary<string, object> parameters);
    }
}

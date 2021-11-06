﻿using BankRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRestApi.Data.Repositories
{
    public interface IUsersRepository
    {
        public Task<User> Get(string login, string password);
    }
}

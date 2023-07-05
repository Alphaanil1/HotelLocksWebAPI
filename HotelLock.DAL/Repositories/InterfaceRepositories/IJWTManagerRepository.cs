using HotelLock.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.DAL.Repositories.InterfaceRepositories
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(UserLoginViewModel users);
    }
}

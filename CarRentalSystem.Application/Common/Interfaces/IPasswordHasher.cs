using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Application.Common.Interfaces
{
    public interface IPasswordHasher
    {
        //HASH PASSWORD
        string Hash(string password);

        //VERIFY PASSWORD AGAINST HASH
        bool Verify(string password, string passwordHash);
    }
}

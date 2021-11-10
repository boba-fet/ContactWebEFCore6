using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWebEFCore6.Data
{
    public interface IUserRolesService
    {
        Task EnsureAdminUserRole();
    }
}

using Territory.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Territory.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepositoryAsync<ApplicationUser>
    {
        IEnumerable<ApplicationUser> Search(string searchTerm);
        List<IdentityUserRole<string>> UserRole();
        List<IdentityRole> Role();
    }
}

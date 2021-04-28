using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Territory.DataAccess.Repository
{
    public class ApplicationUserRepository : RepositoryAsync<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public  IEnumerable<ApplicationUser> Search(string searchTerm)
        {
            return _db.ApplicationUsers.Where(u => u.FirstName.Contains(searchTerm.Trim()) ||
                                                               u.LastName.Contains(searchTerm.Trim()) ||
                                                               u.Email.Contains(searchTerm.Trim()) ||
                                                               u.PhoneNumber.Contains(searchTerm.Trim())||
                                                               (u.FirstName + " " + u.LastName).Contains(searchTerm));
        }

        public List<IdentityUserRole<string>> UserRole()
        {
            return _db.UserRoles.ToList();
        }

        public List<IdentityRole> Role()
        {
            return _db.Roles.ToList();
        }
    }
}

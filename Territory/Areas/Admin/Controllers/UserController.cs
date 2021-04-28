using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;
using Territory.Models.ViewModel;
using Territory.Utility;

namespace Territory.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public UserController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ApplicationUser> userList = await _unitOfWork.ApplicationUser.GetAllAsync();
            IEnumerable<ApplicationUser> searchUserList = _unitOfWork.ApplicationUser.Search(SearchTerm);

            var userRole = _unitOfWork.ApplicationUser.UserRole();
            var roles = _unitOfWork.ApplicationUser.Role();

            foreach (var user in userList)
            {
                IdentityUserRole<string> first = null;
                foreach (var u in userRole)
                {
                    if (u.UserId == user.Id)
                    {
                        first = u;
                        break;
                    }
                }

                var roleId = first.RoleId;
                IdentityRole first1 = null;
                foreach (var r in roles)
                {
                    if (r.Id == roleId)
                    {
                        first1 = r;
                        break;
                    }
                }

                user.Role = first1.Name;
            }

            UserVM userVm = new UserVM()
            {
                ApplicationUser = userList,
                SearchTerm = SearchTerm
            };

            if (userVm.SearchTerm != null)
            {
                userVm.ApplicationUser = searchUserList;
                return View(userVm);
            }

            return View(userVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);

            //if (objFromDb == null)
            //{
            //    return Json(new { success = false, message = "Error while Locking/Unlocking" });
            //}

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and will be unlocked
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                //user is unlocked and will be locked
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UserTerritoryList(string id)
        {
            var publisherTerritories = await _unitOfWork.PublisherTerritory.GetAllAsync(t =>
                t.ApplicationUserId == id, includeProperties:"Territory");

            UserTerritoryListVM userTerritoryListVm = new UserTerritoryListVM()
            {
                PublisherTerritories = publisherTerritories,
                ApplicationUsers = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == id)
            };

            return View(userTerritoryListVm);
        }
    }
}
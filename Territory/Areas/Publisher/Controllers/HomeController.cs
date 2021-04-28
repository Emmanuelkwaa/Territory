using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;
using Territory.Models.ViewModel;

namespace Territory.Areas.Publisher.Controllers
{
    [Area("Publisher")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        //[BindProperty]
        //public int Filter { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var allTerritory = await _unitOfWork.Territory.GetAllAsync();
            var allSearchTerritories = _unitOfWork.Territory.Search(SearchTerm);
            var applicationUser = new ApplicationUser();

            if (claim != null)
            {
                applicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claim.Value);
            }

            HomeVM homeVm = new HomeVM()
            {
                Territory = allTerritory,
                ApplicationUsers = applicationUser,
                SearchTerm = SearchTerm
            };

            if (applicationUser != null)
            {
                homeVm.ApplicationUsers = applicationUser;
            }

            if (homeVm.SearchTerm != null)
            {
                homeVm.Territory = allSearchTerritories;

                foreach (var territory in allSearchTerritories)
                {
                    if (territory.Title != null)
                    {
                        territory.Title = territory.Title.ToUpper();
                    }

                    territory.FirstName = territory.FirstName.ToUpper();
                    territory.LastName = territory.LastName.ToUpper();
                    territory.Street = territory.Street.ToUpper();

                    if (territory.Apartment != null)
                    {
                        territory.Apartment = territory.Apartment.ToUpper();
                    }
                    territory.City = territory.City.ToUpper();
                    territory.State = territory.State.ToUpper();
                }

                return View(homeVm);
            }
            else
            {
                foreach (var territory in allTerritory)
                {
                    if (territory.Title != null)
                    {
                        territory.Title = territory.Title.ToUpper();
                    }

                    if (territory.FirstName != null)
                    {
                        territory.FirstName = territory.FirstName.ToUpper();
                    }
                    
                    if (territory.LastName != null)
                    {
                        territory.LastName = territory.LastName.ToUpper();
                    }
                    
                    if (territory.Street != null)
                    {
                        territory.Street = territory.Street.ToUpper();
                    }

                    if (territory.Apartment != null)
                    {
                        territory.Apartment = territory.Apartment.ToUpper();
                    }
                    
                    if (territory.City != null)
                    {
                        territory.City = territory.City.ToUpper();
                    }
                    
                    if (territory.State != null)
                    {
                        territory.State = territory.State.ToUpper();
                    }
                }

                return View(homeVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int filter)
        {
            // filter = 1 => Not Taken
            // filter = 2 => Taken
            // filter = 3 => With Phone Number
            // filter = 4 => No Phone Number

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var allTerritory = await _unitOfWork.Territory.GetAllAsync();
            var applicationUser = new ApplicationUser();

            var filter1 = await _unitOfWork.Territory.GetAllAsync(t => t.IsTaken == false);
            var filter2 = await _unitOfWork.Territory.GetAllAsync(t => t.IsTaken == true);
            var filter3 = await _unitOfWork.Territory.GetAllAsync(t => t.PhoneNumber != null);
            var filter4 = await _unitOfWork.Territory.GetAllAsync(t => t.PhoneNumber == null);

            if (claim != null)
            {
                applicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == claim.Value);
            }

            HomeVM homeVm = new HomeVM()
            {
                Territory = allTerritory,
                ApplicationUsers = applicationUser,
                SearchTerm = SearchTerm
            };

            switch (filter)
            {
                case 1:
                    homeVm.Territory = filter1;
                    break;

                case 2:
                    homeVm.Territory = filter2;
                    break;

                case 3:
                    homeVm.Territory = filter3;
                    break;
                case 4:
                    homeVm.Territory = filter4;
                    break;
            }

            return View(homeVm);
        }

        public async Task<IActionResult> Details(int id)
        {
            Models.Territory territory = await _unitOfWork.Territory.GetFirstOrDefaultAsync(t => t.Id == id);

            PublisherTerritory publisherTerritoryObj = new PublisherTerritory()
            {
                Territory = territory,
                TerritoryId = territory.Id
            };

            return View(publisherTerritoryObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(PublisherTerritory publisherTerritory)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                publisherTerritory.ApplicationUserId = claim.Value;

                var applUser =
                    await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == publisherTerritory.ApplicationUserId);
                var territoryFromDb =
                    await _unitOfWork.Territory.GetFirstOrDefaultAsync(t => t.Id == publisherTerritory.TerritoryId);

                if (territoryFromDb == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                
                if (territoryFromDb.IsTaken == true)
                {
                    _unitOfWork.Territory.Update(territoryFromDb);
                    return RedirectToAction(nameof(Index));
                }

                //Look in DB to see if that publisher territory exist
                var publisherTerritoryFromDb = await _unitOfWork.PublisherTerritory.GetFirstOrDefaultAsync(
                    t => t.ApplicationUserId == claim.Value && t.TerritoryId == publisherTerritory.TerritoryId, 
                    includeProperties: "Territory");

                if(publisherTerritoryFromDb == null)
                {
                    PublisherTerritory publisherTerritoryToBeAdded = new PublisherTerritory();
                    publisherTerritoryToBeAdded.Territory = territoryFromDb;
                    publisherTerritoryToBeAdded.ApplicationUser = applUser;
                    publisherTerritoryToBeAdded.TerritoryId = publisherTerritory.TerritoryId;
                    publisherTerritoryToBeAdded.ApplicationUserId = claim.Value;
                    publisherTerritoryToBeAdded.Territory.IsTaken = true;
                    publisherTerritoryToBeAdded.Territory.CurrentPub =
                        applUser.FirstName + " " +
                        applUser.LastName;
                    publisherTerritoryToBeAdded.Territory.DateCurrentPub = DateTime.Now;

                    await _unitOfWork.PublisherTerritory.AddAsync(publisherTerritoryToBeAdded);
                }
                else
                {
                    _unitOfWork.PublisherTerritory.Update(publisherTerritoryFromDb);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var territory = await _unitOfWork.Territory.GetFirstOrDefaultAsync(t => t.Id == publisherTerritory.TerritoryId);

                var publisherTerritoryObj = new PublisherTerritory()
                {
                    Territory = territory,
                    TerritoryId = territory.Id
                };

                _unitOfWork.Save();
                return View(publisherTerritoryObj);
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

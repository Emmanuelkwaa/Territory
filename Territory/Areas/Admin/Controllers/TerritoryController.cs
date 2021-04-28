 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models.ViewModel;
using Territory.Utility;
using Territory = Territory.Models.Territory;

namespace Territory.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class TerritoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public TerritoryController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string search)
        {
            var allTerritory = await _unitOfWork.Territory.GetAllAsync();
            var allSearchTerritories = _unitOfWork.Territory.Search(SearchTerm);

            TerritoryVM territoryVm = new TerritoryVM()
            {
                Territory = allTerritory,
                SearchTerm = SearchTerm
            };

            if (territoryVm.SearchTerm != null)
            {
                territoryVm.Territory = allSearchTerritories;

                foreach (var territory in allSearchTerritories)
                {
                    territory.Title = territory.Title.ToUpper();
                    territory.FirstName = territory.FirstName.ToUpper();
                    territory.LastName = territory.LastName.ToUpper();
                    territory.Street = territory.Street.ToUpper();
                    territory.Apartment = territory.Apartment.ToUpper();
                    territory.City = territory.City.ToUpper();
                    territory.State = territory.State.ToUpper();
                }

                return View(territoryVm);
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

                return View(territoryVm);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormFile file)
        {
            // var territoriesFromDb = _unitOfWork.Territory.GetAll().ToList();

            string webRootPath = _hostEnvironment.WebRootPath;

            if (file == null)
            {
                return RedirectToAction(nameof(Index));
            }

            string fileName = $"{webRootPath}//files//{file.FileName}";
            var uploads = Path.Combine(webRootPath, @"files");
            var path = Path.Combine(uploads, fileName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            var territoryList = GetTerritoryList(file.FileName);

            if (territoryList != null)
            {
                foreach (var territory in territoryList)
                {
                    await _unitOfWork.Territory.AddAsync(territory);
                }
                
                _unitOfWork.Save();
            }
            
            // Get all territory and group the duplicates then delete them
            var territoriesFromDb = await _unitOfWork.Territory.GetAllAsync();
            var territories = territoriesFromDb.GroupBy(t => new
            {
                //Group by this variables
                t.Title, t.FirstName, t.LastName,
                t.Street, t.Apartment, t.City, t.State, t.Zipcode, t.PhoneNumber
            }).SelectMany(grp => grp.Skip(1));
            //Remove the grouped duplicate territories
            await _unitOfWork.Territory.RemoveRangeAsync(territories);
            
            _unitOfWork.Save();

            var filePath = Path.Combine(webRootPath, Path.Combine(uploads, fileName));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction(nameof(Index));
        }

        private List<Models.Territory> GetTerritoryList(string fileName)
        {
            List<Models.Territory> territoryList = new List<Models.Territory>();

            var fName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "//" + fileName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(fName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        var territory = new Models.Territory();
                        
                        if (reader.GetValue(0) != null)
                        {
                            territory.Title = reader.GetValue(0).ToString();
                        }
                        
                        if (reader.GetValue(1) != null)
                        {
                            territory.FirstName = reader.GetValue(1).ToString();
                        }
                        
                        if (reader.GetValue(2) != null)
                        {
                            territory.LastName = reader.GetValue(2).ToString();
                        }
                        
                        if (reader.GetValue(3) != null)
                        {
                            territory.Street = reader.GetValue(3).ToString();
                        }
                        
                        if (reader.GetValue(4) != null)
                        {
                            territory.Apartment = reader.GetValue(4).ToString();
                        }
                        
                        if (reader.GetValue(5) != null)
                        {
                            territory.City = reader.GetValue(5).ToString();
                        }
                        
                        if (reader.GetValue(6) != null)
                        {
                            territory.State = reader.GetValue(6).ToString();
                        }
                        
                        if (reader.GetValue(7) != null)
                        {
                            territory.Zipcode = reader.GetValue(7).ToString();
                        }
                        
                        if (reader.GetValue(8) != null)
                        {
                            territory.PhoneNumber = reader.GetValue(8).ToString();
                        }
                        
                        territoryList.Add(territory);
                        // territoryList.Add(new Models.Territory()
                        // {
                        //     Title = (string)reader.GetValue(0),
                        //     FirstName = reader.GetValue(1).ToString(),
                        //     LastName = reader.GetValue(2).ToString(),
                        //     Street = reader.GetValue(3).ToString(),
                        //     Apartment = (string)reader.GetValue(4),
                        //     City = reader.GetValue(5).ToString(),
                        //     State = reader.GetValue(6).ToString(),
                        //     Zipcode = reader.GetValue(7).ToString(),
                        //     PhoneNumber = (string)reader.GetValue(8)
                        // });
                    }
                }
            }

            return territoryList;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var objFromDb = await _unitOfWork.Territory.GetAsync(id);

            await _unitOfWork.Territory.RemoveAsync(objFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Models.Territory territory = new Models.Territory();

            if (id == null)
            {
                //this is for create
                return View(territory);
            }

            //this is for edit request
            territory = await _unitOfWork.Territory.GetAsync(id.GetValueOrDefault());
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Models.Territory territory)
        {
            if (ModelState.IsValid)
            {
                territory.Title = territory.Title.ToUpper();
                territory.FirstName = territory.FirstName.ToUpper();
                territory.LastName = territory.LastName.ToUpper();
                territory.Street = territory.Street.ToUpper();
                territory.Apartment = territory.Apartment.ToUpper();
                territory.City = territory.City.ToUpper();
                territory.State = territory.State.ToUpper();

                if (territory.Id == 0)
                {
                    await _unitOfWork.Territory.AddAsync(territory);
                }
                else
                {
                    _unitOfWork.Territory.Update(territory);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {

                if (territory.Id != 0)
                {
                    territory = await _unitOfWork.Territory.GetAsync(territory.Id);
                }
            }

            return View(territory);
        }
    }
}

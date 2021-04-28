using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;
using Territory.Models.ViewModel;

namespace Territory.Areas.Publisher.Controllers
{
    [Area("Publisher")]
    [Authorize]
    public class PublisherController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublisherController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var publisherTerritories =
                await _unitOfWork.PublisherTerritory.GetAllAsync(t => t.ApplicationUserId == claim.Value,
                    includeProperties: "Territory,ApplicationUser");

            var publications = await _unitOfWork.Publication.GetAllAsync(p => p.ApplicationUserId == claim.Value,
                includeProperties: "PublisherTerritory,ApplicationUser");

            var note = _unitOfWork.Note.GetAllAsync(n => n.ApplicationUserId == claim.Value,
                includeProperties: "PublisherTerritory,ApplicationUser");

            MyTerritoryVM myTerritoryVm = new MyTerritoryVM
            {
                PublisherTerritories = publisherTerritories,
                Publication = publications,
                Note = await note
            };

            return View(myTerritoryVm);
        }

        public async Task<IActionResult> EditPost(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var territory = await _unitOfWork.PublisherTerritory.GetFirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == claim.Value, includeProperties: "Territory,ApplicationUser");

            var publication = await _unitOfWork.Publication.GetFirstOrDefaultAsync(p => p.PublisherTerritoryId == id && p.ApplicationUserId == claim.Value,
                includeProperties: "ApplicationUser,PublisherTerritory");

            var note = await _unitOfWork.Note.GetFirstOrDefaultAsync(p => p.PublisherTerritoryId == id && p.ApplicationUserId == claim.Value,
                includeProperties: "ApplicationUser,PublisherTerritory");


            EditTerritoryVM editTerritoryVm = new EditTerritoryVM
            {
                PublisherTerritory = territory,
                Publications = publication,
                Note = note
            };

            return View(editTerritoryVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Note(Note note)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EditTerritoryVM editTerritoryVm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            var publisherTerritory =
                await _unitOfWork.PublisherTerritory.GetFirstOrDefaultAsync(p => 
                    p.TerritoryId == editTerritoryVm.PublisherTerritory.TerritoryId);
            
            if (publisherTerritory == null)
            {
                return RedirectToAction(nameof(Index));
            }
            
            var publication =
                await _unitOfWork.Publication.GetFirstOrDefaultAsync(p =>
                    p.PublisherTerritoryId == publisherTerritory.Id);
            
            var territory =
                await _unitOfWork.Territory.GetFirstOrDefaultAsync(t =>
                    t.Id == editTerritoryVm.PublisherTerritory.TerritoryId);
            
            var applicationUser =
                await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u =>
                    u.Id == editTerritoryVm.PublisherTerritory.ApplicationUserId);
            
            var note =
                await _unitOfWork.Note.GetFirstOrDefaultAsync(n => 
                    n.PublisherTerritoryId == publisherTerritory.Id);

            if (editTerritoryVm.PublisherTerritory.Territory.Comment != null)
            {
                territory.Comment = editTerritoryVm.PublisherTerritory.Territory.Comment;
                territory.CommentBy =
                    $"{applicationUser.FirstName} {applicationUser.LastName}";
                _unitOfWork.Territory.Update(territory);
            }
            
            if (note == null && editTerritoryVm.Note.TerritoryNote != null)
            {
                note = new Note();
                note.ApplicationUser = applicationUser;
                note.PublisherTerritory = publisherTerritory;
                note.TerritoryNote = editTerritoryVm.Note.TerritoryNote;
                note.ApplicationUserId = applicationUser.Id;
                note.PublisherTerritoryId = publisherTerritory.Id;
                await _unitOfWork.Note.AddAsync(note);
            }

            if (note != null && editTerritoryVm.Note.TerritoryNote != null)
            {
                note.TerritoryNote = editTerritoryVm.Note.TerritoryNote;
                _unitOfWork.Note.Update(note);
            }

            if (publication == null && editTerritoryVm.Publications.NameOfPublication != null)
            {
                publication = new Publication();
                publication.ApplicationUser = applicationUser;
                publication.DatePlaced = editTerritoryVm.Publications.DatePlaced;
                publication.PublisherTerritory = publisherTerritory;
                publication.ApplicationUserId = applicationUser.Id;
                publication.NameOfPublication = editTerritoryVm.Publications.NameOfPublication;
                publication.PublisherTerritoryId = publisherTerritory.Id;
                await _unitOfWork.Publication.AddAsync(publication);
            }

            if (publication != null && editTerritoryVm.Publications.NameOfPublication != null)
            {
                publication.NameOfPublication = editTerritoryVm.Publications.NameOfPublication;
                publication.DatePlaced = editTerritoryVm.Publications.DatePlaced;
                _unitOfWork.Publication.Update(publication);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var objFromDb = await _unitOfWork.PublisherTerritory.GetFirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser,Territory");

            objFromDb.Territory.IsTaken = false;

            objFromDb.Territory.CurrentPub = null;

            objFromDb.Territory.LastPub =
                objFromDb.ApplicationUser.FirstName + " " + objFromDb.ApplicationUser.LastName;
            objFromDb.Territory.DateLastPub = DateTime.Now;
            
            _unitOfWork.Save();
            await _unitOfWork.PublisherTerritory.RemoveAsync(objFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}

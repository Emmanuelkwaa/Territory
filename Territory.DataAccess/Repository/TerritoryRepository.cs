using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;

namespace Territory.DataAccess.Repository
{
    public class TerritoryRepository : RepositoryAsync<Models.Territory>, ITerritoryRepository
    {
        private readonly ApplicationDbContext _db;

        public TerritoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Models.Territory> Search(string searchTerm)
        {
            return _db.Territories.Where(t => t.Title.Contains(searchTerm.Trim()) ||
                                                      t.FirstName.Contains(searchTerm.Trim()) ||
                                                      t.LastName.Contains(searchTerm.Trim()) ||
                                                      t.Apartment.Contains(searchTerm.Trim()) ||
                                                      t.Street.Contains(searchTerm.Trim()) ||
                                                      t.City.Contains(searchTerm.Trim()) ||
                                                      t.State.Contains(searchTerm.Trim()) ||
                                                      t.Zipcode.Contains(searchTerm.Trim()) ||
                                                      t.PhoneNumber.Contains(searchTerm.Trim()) ||
                                                      (t.FirstName + " " + t.LastName).Contains(searchTerm) ||
                                                      (t.Title + " " + t.FirstName + " " + t.LastName).Contains(searchTerm));
        }

        public void Update(Models.Territory territory)
        {
            var territoryFromDb = _db.Territories.FirstOrDefault(t => t.Id == territory.Id);

            if (territoryFromDb != null)
            {
                territoryFromDb.Street = territory.Street;
                territoryFromDb.Apartment = territory.Apartment;
                territoryFromDb.City = territory.City;
                territoryFromDb.State = territory.State;
                territoryFromDb.Zipcode = territory.Zipcode;
                territoryFromDb.PhoneNumber = territory.PhoneNumber;
                territoryFromDb.Title = territory.Title;
                territoryFromDb.FirstName = territory.FirstName;
                territoryFromDb.LastName = territory.LastName;
                territoryFromDb.IsTaken = territory.IsTaken;
                territoryFromDb.Comment = territory.Comment;
                territoryFromDb.CommentBy = territory.CommentBy;
                territoryFromDb.CurrentPub = territory.CurrentPub;
                territoryFromDb.DateCurrentPub = territory.DateCurrentPub;
                territoryFromDb.LastPub = territory.LastPub;
                territoryFromDb.DateLastPub = territory.DateLastPub;
            }
        }
    }
}

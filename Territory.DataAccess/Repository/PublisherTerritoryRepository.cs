using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;

namespace Territory.DataAccess.Repository
{
    public class PublisherTerritoryRepository : RepositoryAsync<PublisherTerritory>, IPublisherTerritoryRepository
    {
        private readonly ApplicationDbContext _db;
        public PublisherTerritoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PublisherTerritory publisherTerritory)
        {
            var territoryFromDb = _db.PublisherTerritories.FirstOrDefault(t => t.Id == publisherTerritory.Id);

            if (territoryFromDb != null)
            {
                territoryFromDb.TerritoryId = publisherTerritory.TerritoryId;
                territoryFromDb.ApplicationUserId = publisherTerritory.ApplicationUserId;
                territoryFromDb.LastVisited = publisherTerritory.LastVisited;
                territoryFromDb.IsNotGhanaian = publisherTerritory.IsNotGhanaian;
            }
        }
    }
}

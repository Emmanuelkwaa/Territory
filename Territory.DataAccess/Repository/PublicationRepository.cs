using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;

namespace Territory.DataAccess.Repository
{
    public class PublicationRepository : RepositoryAsync<Publication>, IPublicationRepository
    {
        private readonly ApplicationDbContext _db;

        public PublicationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Publication publication)
        {
            var publicationFromDb = _db.Publications.FirstOrDefault(n => n.Id == publication.Id);

            if (publicationFromDb != null)
            {
                publicationFromDb.NameOfPublication = publication.NameOfPublication;
                publicationFromDb.DatePlaced = publication.DatePlaced;
                publicationFromDb.PublisherTerritoryId = publication.PublisherTerritoryId;
                publicationFromDb.ApplicationUserId = publication.ApplicationUserId;
            }
        }
    }
}

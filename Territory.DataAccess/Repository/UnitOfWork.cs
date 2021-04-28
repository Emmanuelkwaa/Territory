using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Territory = new TerritoryRepository(_db);
            PublisherTerritory = new PublisherTerritoryRepository(_db);
            Note = new NoteRepository(_db);
            Publication = new PublicationRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            SP_Call = new SP_Call(_db);
        }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISP_Call SP_Call { get; private set; }
        public ITerritoryRepository Territory { get; private set; }
        public IPublisherTerritoryRepository PublisherTerritory { get; private set; }
        public INoteRepository Note { get; private set; }
        public IPublicationRepository Publication { get; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

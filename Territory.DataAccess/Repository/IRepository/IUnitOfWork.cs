using Territory.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationUserRepository ApplicationUser { get; }
        ISP_Call SP_Call { get; }
        ITerritoryRepository Territory { get; }
        IPublisherTerritoryRepository PublisherTerritory { get; }
        INoteRepository Note { get; }
        IPublicationRepository Publication { get; }

        void Save();
    }
}

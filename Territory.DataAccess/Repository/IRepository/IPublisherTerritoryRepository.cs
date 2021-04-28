using System;
using System.Collections.Generic;
using System.Text;
using Territory.Models;

namespace Territory.DataAccess.Repository.IRepository
{
    public interface IPublisherTerritoryRepository : IRepositoryAsync<PublisherTerritory>
    {
        //IEnumerable<Models.Territory> Search(string searchTerm);
        void Update(PublisherTerritory territory);
    }
}

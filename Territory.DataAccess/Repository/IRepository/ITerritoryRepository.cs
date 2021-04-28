using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.DataAccess.Repository.IRepository
{
    public interface ITerritoryRepository : IRepositoryAsync<Models.Territory>
    {
        IEnumerable<Models.Territory> Search(string searchTerm);
        void Update(Models.Territory territory);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Territory.Models;

namespace Territory.DataAccess.Repository.IRepository
{
    public interface IPublicationRepository : IRepositoryAsync<Publication>
    {
        void Update(Publication publication);
    }
}

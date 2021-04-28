using System;
using System.Collections.Generic;
using System.Text;
using Territory.Models;

namespace Territory.DataAccess.Repository.IRepository
{
    public interface INoteRepository : IRepositoryAsync<Note>
    {
        void Update(Note note);
    }
}

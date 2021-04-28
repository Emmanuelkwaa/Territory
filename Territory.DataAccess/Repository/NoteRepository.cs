using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Territory.DataAccess.Data;
using Territory.DataAccess.Repository.IRepository;
using Territory.Models;

namespace Territory.DataAccess.Repository
{
    public class NoteRepository : RepositoryAsync<Note>, INoteRepository
    {
        private readonly ApplicationDbContext _db;

        public NoteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Note note)
        {
            var notesFromDb = _db.Notes.FirstOrDefault(n => n.Id == note.Id);

            if (notesFromDb != null)
            {
                notesFromDb.TerritoryNote = note.TerritoryNote;
                notesFromDb.PublisherTerritoryId = note.PublisherTerritoryId;
                notesFromDb.ApplicationUserId = note.ApplicationUserId;
            }
        }
    }
}

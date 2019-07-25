using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;
using Microsoft.EntityFrameworkCore;

namespace doneillspa.DataAccess
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationContext _context;

        public NoteRepository(ApplicationContext context)
        {
            _context = context;
        }

        public void DeleteNote(TimesheetNote not)
        {
            _context.Entry(not).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public TimesheetNote GetNoteById(long id)
        {
            return _context.TimesheetNote
                        .Where(b => b.Id == id)
                        .FirstOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Models;

namespace doneillspa.DataAccess
{
    public interface INoteRepository
    {
        TimesheetNote GetNoteById(long id);
        void DeleteNote(TimesheetNote note);
    }
}

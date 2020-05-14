using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services;
using doneillspa.Services.Calendar;
using doneillspa.Services.Email;

namespace doneillspa.Models.State
{
    public interface IHolidayRequestState
    {
        void Approve();
        void Reject();
    }
}

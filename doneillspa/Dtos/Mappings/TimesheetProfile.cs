using AutoMapper;
using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos.Mappings
{
    public class TimesheetProfile : Profile
    {
        public TimesheetProfile()
        {
            CreateMap<EmailNotification, EmailNotificationDto>();
            CreateMap<Certification, CertificationDto>();
            CreateMap<TimesheetNote, TimesheetNoteDto>();
        }
    }
}

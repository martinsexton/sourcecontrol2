using AutoMapper;
using doneillspa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Dtos.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmailNotification, EmailNotificationDto>();
            CreateMap<TimesheetNote, TimesheetNoteDto>();
            CreateMap<Project, ProjectDto>()
                .ForMember(dest =>
                dest.Client,
                opt => opt.MapFrom(src => src.OwningClient.Name));

        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Events.Domain.Entities;
using Events.Authentications;
using Events.DTOs.HelperModels;
using Events.DTOs.DTOs;
using Events.Authentications.AuthModels;

namespace Events.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.Entities.Event, EventDTO>().
                ForMember(dest => dest.CategoryStr, src => src.MapFrom(x => x.Category.ToString()));
                //ForMember(dest => dest.FreePlaces, src => src.MapFrom(x => x.FreePlacesCount()));

            CreateMap<EventDTO, Domain.Entities.Event>().
                ForMember(dest => dest.Category, src => src.MapFrom(x => (Domain.Entities.EventCategory)Enum.Parse(typeof(Domain.Entities.EventCategory), x.CategoryStr)));

            CreateMap<Authentications.AuthModels.RegistrationModel, Domain.Entities.User>().
                ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Authentications.AuthModels.AuhorizationModel, Domain.Entities.User>().ReverseMap();

            CreateMap<User, GetTokenRequestModel>();

            CreateMap<UpdateEventModel, Domain.Entities.Event>().
                ForMember(dest => dest.Category, src => src.MapFrom(x => (Domain.Entities.EventCategory)Enum.Parse(typeof(Domain.Entities.EventCategory), x.Category)));

            CreateMap<Notification, NotificationDTO>();

        }
    }
}

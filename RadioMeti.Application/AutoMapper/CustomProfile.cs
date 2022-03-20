using AutoMapper;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Domain.Entities.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.AutoMapper
{
    public class CustomProfile:Profile
    {
        public CustomProfile()
        {
            CreateMap<CreateArtistDto, Artist>().ReverseMap();
            CreateMap<EditArtistDto, Artist>().ReverseMap();
        }

    }
}

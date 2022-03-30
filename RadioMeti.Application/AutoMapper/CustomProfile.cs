using AutoMapper;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Album.Create;
using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
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
            #region Artist
            CreateMap<CreateArtistDto, Artist>().ReverseMap();
            CreateMap<EditArtistDto, Artist>().ReverseMap();

            #endregion
            #region Music
            CreateMap<CreateSingleTrackDto, Music>().ReverseMap();
            CreateMap<EditSingleTrackDto, Music>().ReverseMap();
            #endregion
            #region Album
            CreateMap<CreateAlbumDto, Album>().ReverseMap();
            CreateMap<EditAlbumDto, Album>().ReverseMap();

            #endregion
        }

    }
}

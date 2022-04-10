using AutoMapper;
using RadioMeti.Application.DTOs.Admin.Artists.Create;
using RadioMeti.Application.DTOs.Admin.Artists.Edit;
using RadioMeti.Application.DTOs.Admin.Dj.Create;
using RadioMeti.Application.DTOs.Admin.Dj.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Album.Create;
using RadioMeti.Application.DTOs.Admin.Music.Album.Edit;
using RadioMeti.Application.DTOs.Admin.Music.AlbumMusic.Create;
using RadioMeti.Application.DTOs.Admin.Music.AlbumMusic.Edit;
using RadioMeti.Application.DTOs.Admin.Music.Single.Create;
using RadioMeti.Application.DTOs.Admin.Music.Single.Edit;
using RadioMeti.Application.DTOs.Admin.Prodcast.Create;
using RadioMeti.Application.DTOs.Admin.Prodcast.Edit;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Domain.Entities.Prodcast;
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
            #region AlbumMusic
            CreateMap<CreateAlbumMusicDto, Music>().ReverseMap();
            CreateMap<EditAlbumMusicDto, Music>().ReverseMap();
            #endregion

            #region dj
            CreateMap<CreateDjDto,Dj>().ReverseMap();
            CreateMap<EditDjDto, Dj>().ReverseMap();
            #endregion

            #region Prodcast
            CreateMap<CreateProdcastDto, Prodcast>().ReverseMap();
            CreateMap<EditProdcastDto, Prodcast>().ReverseMap();
            #endregion
        }

    }
}

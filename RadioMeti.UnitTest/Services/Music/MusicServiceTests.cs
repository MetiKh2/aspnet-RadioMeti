using AutoMapper;
using Moq;
using NUnit.Framework;
using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Services;
using RadioMeti.Domain.Entities.Music;
using RadioMeti.Persistance.Repository;

namespace RadioMeti.UnitTest.Services.Music
{
    [TestFixture]
    public class MusicServiceTests
    {
        private IMusicService _musicService;
        private Mock<IGenericRepository<Domain.Entities.Music.Music>> _musicRepository;
        private Mock<IGenericRepository<ArtistMusic>> _artistMusicRepository;
        private Mock<IGenericRepository<Artist>> _artistRepository;
        private Mock<IGenericRepository<Album>> _albumRepository;
        private Mock<IGenericRepository<ArtistAlbum>> _artistAlbumRepository;
        private Mock<IMapper> _mapper;
        [SetUp]
        public void SetUp()
        {
            _musicRepository = new Mock<IGenericRepository<Domain.Entities.Music.Music>>();
            _artistMusicRepository = new Mock<IGenericRepository<ArtistMusic>>();
            _albumRepository = new Mock<IGenericRepository<Album>>();
            _artistAlbumRepository = new Mock<IGenericRepository<ArtistAlbum>>();
            _artistRepository=new Mock<IGenericRepository<Artist>>();
            _mapper = new Mock<IMapper>();
            _musicService = new MusicService(_musicRepository.Object, _mapper.Object, _artistMusicRepository.Object, _artistRepository.Object, _albumRepository.Object, _artistAlbumRepository.Object);
        }
      
    }
}

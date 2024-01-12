using SpotifyProject.Models;
using SpotifyProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.ViewModels
{


    public class VideoPageVM
    {
        public PlaylistService PlaylistService { get; set; }
        public List<Playlist> Playlists { get; set; }

        // contructor
        public VideoPageVM()
        {
            PlaylistService = new PlaylistService(DatabaseConnection.GetInstance().getConnection());
        }
        public void LoadPlaylists()
        {
            Playlists = PlaylistService.GetAllPlaylist(PlaylistType.Video);
        }

    }

}

using SpotifyProject.Models;
using SpotifyProject.Services;
using SpotifyProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpotifyProject.ViewModels
{
    public class MusicPageVM
    {
        public PlaylistService PlaylistService { get; set; }
        public List<Playlist> Playlists { get; set; }
        // Property to represent the Frame
        public MusicPageVM()
        {
            PlaylistService = new PlaylistService(DatabaseConnection.GetInstance().getConnection());     
        }

        public void LoadPlaylists()
        {
            Playlists = PlaylistService.GetAllPlaylist(PlaylistType.Song);
        }
   
    }
}

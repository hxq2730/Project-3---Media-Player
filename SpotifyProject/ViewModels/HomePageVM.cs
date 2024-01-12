using SpotifyProject.Models;
using SpotifyProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.ViewModels
{
    public class HomePageVM
    {
        public PlaylistService PlaylistService { get; set; }
        public MediaService MediaService { get; set; }
        public List<Playlist> PlaylistSong { get; set; }
        public List<Playlist> PlaylistVideo { get; set; }
        public List<RecentView> RecentViews { get; set; }
        public List<Recents> Recents { get; set; }
        public HomePageVM()
        {
            PlaylistService = new PlaylistService(DatabaseConnection.GetInstance().getConnection());
            MediaService = new MediaService(DatabaseConnection.GetInstance().getConnection());
            RecentViews = new List<RecentView>();
            PlaylistSong = new List<Playlist>();
            PlaylistVideo = new List<Playlist>();
        }

        // Property to represent the Frame

        public void LoadPlaylists()
        {
            PlaylistSong = PlaylistService.GetAllPlaylist(PlaylistType.Song);
            PlaylistVideo = PlaylistService.GetAllPlaylist(PlaylistType.Video);
            LoadRecents();
        }

        public void LoadRecents()
        {
            Recents = MediaService.GetRecents();
            RecentViews  = new List<RecentView>();
            foreach (var recent in Recents)
            {
                var mediaItem = MediaService.GetMediaItemById(recent.MediaItemId);
                var playlist = PlaylistService.GetPlaylist(recent.PlaylistId);
                if (mediaItem != null && playlist != null)
                {
                    RecentViews.Add(new RecentView(mediaItem.Title, mediaItem.Type, playlist.Name, mediaItem.Path, playlist.Id, mediaItem.Id, playlist.Image));

                }
            }
        }

    }
}

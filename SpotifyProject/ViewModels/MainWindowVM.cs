using SpotifyProject.Models;
using SpotifyProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.ViewModels
{
    public class MainWindowVM
    {
        public PlaylistService PlaylistService { get; set; }
        public MediaService MediaService { get; set; }

        public List<Recents> Recents { get; set; }

        public List<RecentView> RecentViews { get; set; } // list recents show on UI

        public MainWindowVM()
        {
            PlaylistService = new PlaylistService(DatabaseConnection.GetInstance().getConnection());
            MediaService = new MediaService(DatabaseConnection.GetInstance().getConnection());
            Recents = new List<Recents>();
            RecentViews = new List<RecentView>();
        }

        // load data recents from database
        public void LoadRecents()
        {
            Recents = MediaService.GetRecents();
            RecentViews = new List<RecentView>();
            foreach (var recent in Recents)
            {
                var mediaItem = MediaService.GetMediaItemById(recent.MediaItemId);
                var playlist = PlaylistService.GetPlaylist(recent.PlaylistId);
                if(mediaItem != null && playlist != null)              
                {
                    RecentViews.Add(new RecentView(mediaItem.Title, mediaItem.Type, playlist.Name, mediaItem.Path, playlist.Id, mediaItem.Id, playlist.Image));
                    
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.Models
{
    public class RecentView : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public MediaType Type { get; set; }
        public string PlaylistName { get; set; }
        public int PlaylistId { get; set; }
        public int MediaItemId { get; set; }
        public string Path { get; set; }
        public string Image { get; set; }

        public RecentView(string title, MediaType type, string playlistName, string path, int playlistId, int mediaItemId, string image)
        {
            Title = title;
            Type = type;
            PlaylistName = playlistName;
            Path = path;
            PlaylistId = playlistId;
            MediaItemId = mediaItemId;
            Image = image;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

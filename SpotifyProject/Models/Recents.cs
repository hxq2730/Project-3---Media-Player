using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.Models
{
    public class Recents : INotifyPropertyChanged
    {
       
        public int Id { get; set; }
        public int PlaylistId { get; set; }
        public int MediaItemId { get; set; }

        public Recents(int id, int playlistId, int mediaItemId)
        {
            Id = id;
            PlaylistId = playlistId;
            MediaItemId = mediaItemId;
        }
        public Recents(int playlistId, int mediaItemId)
        {
            PlaylistId = playlistId;
            MediaItemId = mediaItemId;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

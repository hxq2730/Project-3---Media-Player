using SpotifyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.Helper
{
    public static class MediaHelper
    {

        public static List<Song> castMediaItemsToSongs(List<MediaItem> mediaItems)
        {
            List<Song> songs = new List<Song>();
            foreach (MediaItem mediaItem in mediaItems)
            {
                if (mediaItem.Type == MediaType.Song)
                {
                    songs.Add((Song)mediaItem);
                }
            }
            return songs;
        }

        public static List<Video> castMediaItemsToVideos(List<MediaItem> mediaItems)
        {
            List<Video> videos = new List<Video>();
            foreach (MediaItem mediaItem in mediaItems)
            {
                if (mediaItem.Type == MediaType.Video)
                {
                    videos.Add((Video)mediaItem);
                }
            }
            return videos;
        }

    }
}

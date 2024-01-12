using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SpotifyProject.Models
{
    public class Song : MediaItem
    {
        
        public string Album { get; set; }
        public string Year { get; set; }
        public string Genre { get; set;}
        public string Length { get; set; }
        public Song(int Id, string title, string artist, string album, string year, string genre, string length, string path) : base(Id,title, artist, MediaType.Song, path)
        {
            Album = album;
            Year = year;
            Genre = genre;
            Length = length;
        }
        public Song(string title, string artist, string album, string year, string genre, string length, string path) : base(title, artist, MediaType.Song, path)
        {
            Album = album;
            Year = year;
            Genre = genre;
            Length = length;
        }

    }

}

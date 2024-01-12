using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyProject.Models
{
    public class Video : MediaItem
    {
        public string Date { get; set; }
        public string Length { get; set; }

        public Video(int Id, string title, string artist, string path, string date, string length) : base(Id, title, artist, MediaType.Video, path)
        {
            Date = date;
            Length = length;
        }
        public Video(string title, string artist, string path, string date, string length) : base(title, artist, MediaType.Video, path)
        {
            Date = date;
            Length = length;
        }
    }

}

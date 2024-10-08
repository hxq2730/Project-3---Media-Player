﻿using System.Collections.Generic;
using System.ComponentModel;

namespace SpotifyProject.Models
{
    public class Playlist : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public List<MediaItem> MediaItems { get; set; }
        public PlaylistType Type { get; set; }

        public Playlist(string name, string image, string description, PlaylistType type)
        {
            Name = name;
            MediaItems = new List<MediaItem>();
            Image = image;
            Description = description;
            Type = type;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void AddMediaItem(MediaItem mediaItem)
        {
            MediaItems.Add(mediaItem);
        }
        public void removeMediaItem(MediaItem mediaItem)
        {
            MediaItems.Remove(mediaItem);
        }
    }
    public enum PlaylistType
    {
        Song = 1,
        Video = 2,
        Unknown = 3
    }

}

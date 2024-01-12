
using SpotifyProject.Models;
using System;
using WMPLib;

namespace SpotifyProject.Services
{

    public enum RepeatMode
    {
        Off = 0,
        One = 1,
        All = 2,
    }

    public enum PlayerState
    {
        Playing = 3,
        Paused = 2,
        Stopped = 1
    }

    public static class PlayerMedia
    {

        public static WindowsMediaPlayer player = new WindowsMediaPlayer();
        public static RepeatMode RepeatMode { get; set; } = RepeatMode.Off;
        public static bool ShuffleMode { get; set; } = false;
        public static bool IsPlaying { get; set; } = false;
        public static bool IsPaused { get; set; } = false;
        public static bool IsStopped { get; set; } = true;
        public static bool IsMuted { get; set; } = false;
        public static int Volume { get; set; } = 50;
        public static int CurrentSongIndex { get; set; } = 0;
        public static Playlist CurrentPlaylist { get; set; }
        public static Song CurrentSong { get; set; }
        public static Video CurrentVideo { get; set; }
        public static int CurrentSongPosition { get; set; } = 0;
        public static int CurrentSongDuration { get; set; } = 0;
        public static int CurrentSongProgress { get; set; } = 0;
        public static int CurrentSongProgressPercentage { get; set; } = 0;

        public static void PlaySong(string path)
        {
            player.URL = path;
            player.controls.play();
            IsPlaying = true;
            IsPaused = false;
            IsStopped = false;            
        }
        public static void PauseSong()
        {
            player.controls.pause();
            IsPlaying = false;
            IsPaused = true;
            IsStopped = false;
        }

        public static void StopSong()
        {
            player.controls.stop();
            IsPlaying = false;
            IsPaused = false;
            IsStopped = true;
        }


        public static void MuteSong()
        {
            player.settings.mute = true;
            IsMuted = true;
        }

        public static void UnmuteSong()
        {
            player.settings.mute = false;
            IsMuted = false;
        }

        public static void SetVolume(int volume)
        {
            player.settings.volume = volume;
            Volume = volume;
        }

        public static void SetSongPosition(int position)
        {
            player.controls.currentPosition = position;
            CurrentSongPosition = position;
        }

        public static void SetRepeatMode(RepeatMode repeatMode)
        {
            switch (repeatMode)
            {
                case RepeatMode.Off:
                    player.settings.setMode("loop", false);
                    RepeatMode = RepeatMode.Off;
                    break;
                case RepeatMode.One:
                    player.settings.setMode("loop", true);
                    RepeatMode = RepeatMode.One;
                    break;
                case RepeatMode.All:
                    player.settings.setMode("loop", true);
                    RepeatMode = RepeatMode.All;
                    break;
            }
        }

        public static void SetShuffleMode(bool shuffleMode)
        {
            if (shuffleMode)
            {
                player.settings.setMode("shuffle", true);
                ShuffleMode = true;
            }
            else
            {
                player.settings.setMode("shuffle", false);
                ShuffleMode = false;
            }
        }

        //current song duration
        public static int GetCurrentSongDuration()
        {
            return (int)player.controls.currentItem.duration;
        }

        // get current song position
        public static int GetCurrentSongPosition()
        {
            return (int)player.controls.currentPosition;
        }


        // contunue song
        public static void ContinueSong()
        {
            player.controls.play();
            IsPlaying = true;
            IsPaused = false;
            IsStopped = false;
        }

        // next song
        public static void NextSong()
        {
            if (CurrentPlaylist != null)
            {
                if (CurrentSongIndex < CurrentPlaylist.MediaItems.Count - 1)
                {
                    CurrentSongIndex++;
                    CurrentSong = CurrentPlaylist.MediaItems[CurrentSongIndex] as Song;
                    PlaySong(CurrentSong.Path);
                }
                else
                {
                    CurrentSongIndex = 0;
                    CurrentSong = CurrentPlaylist.MediaItems[CurrentSongIndex] as Song;
                    PlaySong(CurrentSong.Path);
                }
            }
        }
      
        // previous song
        public static void PreviousSong()
        {
            if (CurrentPlaylist != null)
            {
                if (CurrentSongIndex > 0)
                {
                    CurrentSongIndex--;
                    CurrentSong = CurrentPlaylist.MediaItems[CurrentSongIndex] as Song;
                    PlaySong(CurrentSong.Path);
                }
                else
                {
                    CurrentSongIndex = CurrentPlaylist.MediaItems.Count - 1;
                    CurrentSong = CurrentPlaylist.MediaItems[CurrentSongIndex] as Song;
                    PlaySong(CurrentSong.Path);
                }
            }
        }

        // random song
        public static void RandomSong()
        {
            if (CurrentPlaylist != null)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, CurrentPlaylist.MediaItems.Count - 1);
                CurrentSongIndex = randomIndex;
                CurrentSong = CurrentPlaylist.MediaItems[CurrentSongIndex] as Song;
                PlaySong(CurrentSong.Path);
            }
        }

    }
}

using SpotifyProject.Models;
using SpotifyProject.Services;
using SpotifyProject.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SpotifyProject.Views
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private HomePageVM homePageVM;
        private UIElement _bottomBarMusic;

        public HomePage(UIElement bottomBarMusic)
        {
            InitializeComponent();
            homePageVM = new HomePageVM();
            _bottomBarMusic = bottomBarMusic;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            homePageVM.LoadPlaylists();
            this.DataContext = homePageVM;
            playlistSong.ItemsSource = homePageVM.PlaylistSong;
            playlistVideo.ItemsSource = homePageVM.PlaylistVideo;
        }

        private void panelChooseListSong_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Playlist? selectedPlaylist = ((FrameworkElement)sender).DataContext as Playlist;

            if (selectedPlaylist != null)
            {
                NavigationService.Navigate(new ListMusic(selectedPlaylist, _bottomBarMusic));
            }
        }
        private void panelChooselistVideo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Playlist? selectedPlaylist = ((FrameworkElement)sender).DataContext as Playlist;

            if (selectedPlaylist != null)
            {
                NavigationService.Navigate(new PlaylistViewPage(selectedPlaylist, _bottomBarMusic));
            }
        }

        private void panelChoosePlaylistVideo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Playlist? selectedPlaylist = ((FrameworkElement)sender).DataContext as Playlist;

            if (selectedPlaylist != null)
            {
                NavigationService.Navigate(new PlaylistViewPage(selectedPlaylist, _bottomBarMusic));
            }
        }

        private void panelChooseRecent_MouseDown(object sender, MouseButtonEventArgs e)
        {

            // select song and play
            RecentView? selected = ((FrameworkElement)sender).DataContext as RecentView;

            if (selected != null)
            {
                PlayerMedia.CurrentPlaylist = homePageVM.PlaylistService.GetPlaylist(selected.PlaylistId);
                if (PlayerMedia.CurrentPlaylist.Type == PlaylistType.Song)
                {
                    var songIndex = 0;
                    Song CurrentSong = null;
                    foreach (var song in PlayerMedia.CurrentPlaylist.MediaItems)
                    {
                        if (song.Id == selected.MediaItemId)
                        {
                            CurrentSong = (Song?)song;
                            break;
                        }
                        songIndex++;
                    }

                    if (CurrentSong != null)
                    {
                        PlayerMedia.CurrentSong = CurrentSong;
                        PlayerMedia.CurrentSongIndex = songIndex;
                        PlayerMedia.PlaySong(CurrentSong.Path);
                    }
                    else
                    {
                        // reload recents
                        homePageVM.LoadRecents();
                        MessageBox.Show("File not found!");

                    }


                }
                else if (PlayerMedia.CurrentPlaylist.Type == PlaylistType.Video)
                {
                    var videoIndex = 0;
                    Video CurrentVideo = null;
                    foreach (var video in PlayerMedia.CurrentPlaylist.MediaItems)
                    {
                        if (video.Id == selected.MediaItemId)
                        {
                            CurrentVideo = (Video?)video;
                            break;
                        }
                        videoIndex++;
                    }

                    if (CurrentVideo != null)
                    {
                        PlayerMedia.CurrentVideo = CurrentVideo;
                        PlayerMedia.PauseSong();
                        var dialog = new ViewVideo();
                    }
                    else
                    {
                        // reload recents
                        homePageVM.LoadRecents();
                        MessageBox.Show("File not found!");

                    }
                }

            }

        }
    }
}

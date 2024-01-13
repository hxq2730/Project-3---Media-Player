using Id3;
using Microsoft.Win32;
using SpotifyProject.Helper;
using SpotifyProject.Models;
using SpotifyProject.Services;
using SpotifyProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WMPLib;

namespace SpotifyProject.Views
{
    /// <summary>
    /// Interaction logic for ListMusic.xaml
    /// </summary>
    public partial class ListMusic : Page
    {
        private PlaylistPageVM PlaylistPageVM { get; set; }
        private UIElement _bottomBarMusic;

        public ListMusic(Playlist playlist, UIElement bottomBarMusic)
        {
            InitializeComponent();
            this.PlaylistPageVM = new PlaylistPageVM(playlist);
            this.DataContext = playlist;
            _bottomBarMusic = bottomBarMusic;
        }
        public event EventHandler<bool> PlayPauseStateChanged;

        private void OnPlayPauseStateChanged(bool isPlaying)
        {
            PlayPauseStateChanged?.Invoke(this, isPlaying);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            List<Song> lst = MediaHelper.castMediaItemsToSongs(PlaylistPageVM.Playlist.MediaItems);
            listItemsMedia.ItemsSource = lst;
            if (lst.Count <= 1)
            {
                AmountMedia.Text = lst.Count.ToString() + " song";

            }
            else
            {
                AmountMedia.Text = lst.Count.ToString() + " songs";

            }


            // Đăng ký sự kiện từ MainWindow
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                PlayPauseStateChanged += (s, isPlaying) => mainWindow.OnPlayPauseStateChanged(s, isPlaying);
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get the NavigationService of the Frame
            NavigationService navigationService = NavigationService.GetNavigationService(this);

            // Check if it's possible to go back
            if (navigationService.CanGoBack)
            {
                // Go back to the previous page
                navigationService.GoBack();
            }
        }

        private void PlayMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PlaylistPageVM.Playlist.MediaItems.Count == 0)
            {
                MessageBox.Show("There isn't any song in playlist");
                return;
            }

            if (PlaylistPageVM.Playlist.Type == PlaylistType.Song)
            {
                PlayerMedia.CurrentPlaylist = PlaylistPageVM.Playlist;
                PlayerMedia.CurrentSong = (Song)PlaylistPageVM.Playlist.MediaItems[0];
                PlayerMedia.CurrentSongIndex = 0;
                PlayerMedia.PlaySong(PlaylistPageVM.Playlist.MediaItems[0].Path);
                OnPlayPauseStateChanged(PlayerMedia.IsPlaying);
            }
            else if (PlaylistPageVM.Playlist.Type == PlaylistType.Video)
            {
                PlayerMedia.CurrentPlaylist = PlaylistPageVM.Playlist;
                PlayerMedia.CurrentVideo = (Video)PlaylistPageVM.Playlist.MediaItems[0];
                PlayerMedia.CurrentSongIndex = 0;
                //PlayerMedia.PlaySong(PlaylistPageVM.Playlist.MediaItems[0].Path);
                var page = new ViewVideo();
                NavigationService.Navigate(page);
                OnPlayPauseStateChanged(PlayerMedia.IsPlaying);
            }

        }
        private void OptionBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }


        private void OpenDiaLogSong()
        {
            // Khởi tạo OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Thiết lập các thuộc tính cho OpenFileDialog
            openFileDialog.Filter = "Music files (*.mp3)|*.mp3|All files (*.*)|*.*";
            openFileDialog.Title = "Select your song";
            openFileDialog.Multiselect = true; // Cho phép người dùng chọn nhiều file

            // Mở hộp thoại chọn file
            bool? result = openFileDialog.ShowDialog();

            // Xử lý kết quả chọn file
            if (result == true)
            {
                // Lấy danh sách các đường dẫn của các file được chọn
                string[] selectedFiles = openFileDialog.FileNames;

                // Xử lý logic với danh sách các đường dẫn file ở đây
                foreach (string filePath in selectedFiles)
                {
                    // Đọc thông tin ID3 của file MP3
                    using (var mp3 = new Mp3(filePath))
                    {
                        Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);
                        var duration = mp3.Audio.Duration;

                        if (tag != null)
                        {
                            Console.WriteLine(tag);
                            // Lấy thông tin từ các khung ID3
                            string title = tag.Title;
                            string artist = tag.Artists;
                            string album = tag.Album;
                            string year = tag.Year;
                            string genre = tag.Genre;
                            string length = $"{duration.Minutes}:{duration.Seconds}";

                            Song song = new Song(title ?? "Unknown", (artist == "" ? "" : artist), album ?? "Unknown", year ?? "Unknown", genre ?? "Unknown", length ?? "Unknown", filePath);
                            PlaylistPageVM.AddSongToPlaylist(song);
                        }
                        else
                        {
                            // Create Object Song
                            Song song = new Song(Path.GetFileNameWithoutExtension(filePath), "Unknown", "Unknown", "Unknown", "Unknown", $"{duration.Minutes}:{duration.Seconds}", filePath);
                            PlaylistPageVM.AddSongToPlaylist(song);
                        }
                    }

                }

                // reload playlist
                PlaylistPageVM.LoadPlaylist();
                List<Song> lst = MediaHelper.castMediaItemsToSongs(PlaylistPageVM.Playlist.MediaItems);
                listItemsMedia.ItemsSource = lst;

            }

            // Đóng Popup sau khi xử lý
        }

        private void OpenDialogVideo()
        {
            // Khởi tạo OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Thiết lập các thuộc tính cho OpenFileDialog
            openFileDialog.Filter = "Video files (*.mp4)|*.mp4|All files (*.*)|*.*";
            openFileDialog.Title = "Select Video Files";
            openFileDialog.Multiselect = true; // Cho phép người dùng chọn nhiều file

            // Mở hộp thoại chọn file
            bool? result = openFileDialog.ShowDialog();

            // Xử lý kết quả chọn file
            if (result == true)
            {
                // Lấy danh sách các đường dẫn của các file được chọn
                string[] selectedFiles = openFileDialog.FileNames;

                // Xử lý logic với danh sách các đường dẫn file ở đây
                foreach (string filePath in selectedFiles)
                {

                    var info = new FileInfo(filePath);
                    string title = info.Name.Substring(0, info.Name.IndexOf('.'));
                    string artist = "Unknown";

                    DateTime dateCreated = info.CreationTime;

                    string date = dateCreated.ToString();

                    var player = new WindowsMediaPlayer();
                    var clip = player.newMedia(filePath);

                    TimeSpan videoLength = TimeSpan.FromSeconds(clip.duration); // Chuyển đổi từ miliseconds sang giây

                    // Định dạng chuỗi độ dài theo "hh:mm:ss"
                    string length = $"{(int)videoLength.TotalHours:D2}:{videoLength.Minutes:D2}:{videoLength.Seconds:D2}";

                    Video video = new Video(title, artist, filePath, date, length);
                    PlaylistPageVM.AddVideoToPlaylist(video);


                }

                // reload playlist
                PlaylistPageVM.LoadPlaylist();
                List<Video> lst = MediaHelper.castMediaItemsToVideos(PlaylistPageVM.Playlist.MediaItems);
                listItemsMedia.ItemsSource = lst;

            }

            // Đóng Popup sau khi xử lý
        }
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem listViewItem)
            {
                if (PlaylistPageVM.Playlist.Type == PlaylistType.Song)
                {
                    OpenDiaLogSong();
                }
                else
                {
                    OpenDialogVideo();
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftUpdatePlaylistButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListViewItem_PreviewMouseRemoveLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                // Hiển thị ContextMenu
                ((FrameworkElement)sender).ContextMenu.IsOpen = true;
            }
            else if (e.ChangedButton == MouseButton.Left)
            {
                if (PlaylistPageVM.Playlist.Type == PlaylistType.Song)
                {
                    // select song and play
                    Song? selectedSong = ((FrameworkElement)sender).DataContext as Song;

                    if (selectedSong != null)
                    {
                        PlayerMedia.CurrentPlaylist = PlaylistPageVM.Playlist;
                        PlayerMedia.CurrentSong = selectedSong;
                        PlayerMedia.CurrentSongIndex = PlaylistPageVM.Playlist.MediaItems.IndexOf(selectedSong);
                        PlayerMedia.PlaySong(selectedSong.Path);

                        OnPlayPauseStateChanged(PlayerMedia.IsPlaying);

                        PlaylistPageVM.AddRecentFile(selectedSong.Id, PlaylistPageVM.Playlist.Id);

                    }
                }
                else
                {
                    // select song and play
                    Video? selectedVideo = ((FrameworkElement)sender).DataContext as Video;

                    if (selectedVideo != null)
                    {
                        PlayerMedia.CurrentPlaylist = PlaylistPageVM.Playlist;
                        PlayerMedia.CurrentVideo = selectedVideo;
                        PlaylistPageVM.AddRecentFile(selectedVideo.Id, PlaylistPageVM.Playlist.Id);


                        //var dialog = new VideoViewDialog();
                        var page = new ViewVideo();
                        NavigationService.Navigate(page);
                        PlayerMedia.PauseSong();
                        //bool? result = dialog.ShowDialog();

                    }
                }
            }

        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Lấy đối tượng được click vào từ ContextMenu
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    FrameworkElement senderFrameworkElement = contextMenu.PlacementTarget as FrameworkElement;

                    if (senderFrameworkElement != null)
                    {
                        // Lấy đối tượng DataContext từ FrameworkElement
                        object selectedObject = senderFrameworkElement.DataContext;

                        if (selectedObject is Song)
                        {
                            // Xử lý khi đối tượng là Song
                            Song selectedSong = (Song)selectedObject;
                            PlaylistPageVM.DeleteMediaItem(selectedSong.Id);
                            PlaylistPageVM.LoadPlaylist();
                            listItemsMedia.ItemsSource = PlaylistPageVM.Playlist.MediaItems;
                        }
                        else if (selectedObject is Video)
                        {
                            // Xử lý khi đối tượng là Video
                            Video selectedVideo = (Video)selectedObject;
                            PlaylistPageVM.DeleteMediaItem(selectedVideo.Id);
                            PlaylistPageVM.LoadPlaylist();
                            listItemsMedia.ItemsSource = PlaylistPageVM.Playlist.MediaItems;

                        }
                    }
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {

            OpenDiaLogSong();


        }

        private static void Shuffle(List<Song> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Song value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void RandomSongBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            List<Song> lst = MediaHelper.castMediaItemsToSongs(PlaylistPageVM.Playlist.MediaItems);
            Shuffle(lst);

            foreach (var song in lst)
            {
                MediaItem item = song as MediaItem;
                PlaylistPageVM.Playlist.removeMediaItem(item);
            }

            foreach (var song in lst)
            {
                MediaItem item = song as MediaItem;
                PlaylistPageVM.Playlist.AddMediaItem(item);
            }

            PlayerMedia.CurrentPlaylist = PlaylistPageVM.Playlist;
            PlayerMedia.CurrentSong = (Song)PlaylistPageVM.Playlist.MediaItems[0];
            PlayerMedia.CurrentSongIndex = 0;
            if (PlayerMedia.IsPlaying)
            {
                PlayerMedia.PlaySong(PlaylistPageVM.Playlist.MediaItems[0].Path);

                OnPlayPauseStateChanged(PlayerMedia.IsPlaying);

            }




            listItemsMedia.ItemsSource = lst;

        }
    }
}



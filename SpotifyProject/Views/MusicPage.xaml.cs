using SpotifyProject.Models;
using SpotifyProject.ViewModels;
using SpotifyProject.Views.Dialog;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SpotifyProject.Views
{
    /// <summary>
    /// Interaction logic for MusicPage.xaml
    /// </summary>
    public partial class MusicPage : Page
    {
        private MusicPageVM musicPageVM;
        private UIElement _bottomBarMusic;

        public MusicPage(UIElement bottomBarMusic)
        {
            InitializeComponent();
            musicPageVM = new MusicPageVM();
            _bottomBarMusic = bottomBarMusic;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            musicPageVM.LoadPlaylists();
            this.DataContext = musicPageVM;
            listPlaylist.ItemsSource = musicPageVM.Playlists;
        }

        private void NewPlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreatePlaylistDialog();
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string playlistName = dialog.PlaylistName;
                string description = dialog.Description;
                string playlistImagePath = dialog.PlaylistImagePath;

                int insertRow = musicPageVM.PlaylistService.InsertPlaylist(playlistName, playlistImagePath, description, PlaylistType.Song.ToString());
                if (insertRow == 1)
                {
                    MessageBox.Show("Create playlist successfully!");
                }

                // Reload playlist
                musicPageVM.LoadPlaylists();
                listPlaylist.ItemsSource = musicPageVM.Playlists;

            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Playlist? selectedPlaylist = ((FrameworkElement)sender).DataContext as Playlist;

            if (selectedPlaylist != null)
            {
                NavigationService.Navigate(new PlaylistViewPage(selectedPlaylist, _bottomBarMusic));
            }

        }
    }
}

using SpotifyProject.Models;
using SpotifyProject.ViewModels;
using SpotifyProject.Views.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotifyProject.Views
{
    /// <summary>
    /// Interaction logic for VideoPage.xaml
    /// </summary>
    public partial class VideoPage : Page
    {

        private VideoPageVM videoPageVM;
        public VideoPage()
        {
            InitializeComponent();
            videoPageVM = new VideoPageVM();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            videoPageVM.LoadPlaylists();
            this.DataContext = videoPageVM;
            listPlaylist.ItemsSource = videoPageVM.Playlists;
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

                int insertRow = videoPageVM.PlaylistService.InsertPlaylist(playlistName, playlistImagePath, description, PlaylistType.Video.ToString());
                if (insertRow == 1)
                {
                    MessageBox.Show("Create playlist successfully!");
                }

                // Reload playlist
                videoPageVM.LoadPlaylists();
                listPlaylist.ItemsSource = videoPageVM.Playlists;

            }
        }

        private void panelChoosePlaylist_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Playlist? selectedPlaylist = ((FrameworkElement)sender).DataContext as Playlist;

            if (selectedPlaylist != null)
            {
                NavigationService.Navigate(new PlaylistViewPage(selectedPlaylist));
            }
        }
    }
}

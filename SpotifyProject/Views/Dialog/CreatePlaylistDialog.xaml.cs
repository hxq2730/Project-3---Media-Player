using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace SpotifyProject.Views.Dialog
{
    /// <summary>
    /// Interaction logic for CreatePlaylistDialog.xaml
    /// </summary>
    public partial class CreatePlaylistDialog : Window
    {
        public string PlaylistName { get; set;}
        public string Description { get; set; }
        public string PlaylistImagePath { get; set; }
        public CreatePlaylistDialog()
        {
            InitializeComponent();
        }

        private void SavePlaylistBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text != "")
            {
                PlaylistName = txtPlaylistName.Text;
                Description = txtDescription.Text;
                PlaylistImagePath = imgPlaylistImage.Source.ToString();
                DialogResult = true;
            }
        }

        private void imgPlaylistImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                PlaylistImagePath = imagePath;

                // Show the selected image
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                imgPlaylistImage.Source = bitmap;
            }
        }
    }
}

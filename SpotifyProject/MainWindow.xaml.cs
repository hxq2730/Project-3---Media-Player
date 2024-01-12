using SpotifyProject.Models;
using SpotifyProject.Services;
using SpotifyProject.ViewModels;
using SpotifyProject.Views.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using System.Windows.Threading;

namespace SpotifyProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainWindowVM mainWindowVM;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            mainWindowVM = new MainWindowVM();
            this.DataContext = mainWindowVM;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("Views/HomePage.xaml", UriKind.Relative));

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.Tick += _timer_Tick;

            mainWindowVM.LoadRecents();
            recentsList.ItemsSource = mainWindowVM.RecentViews;
        }


        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            
            var stackPanel = sender as StackPanel;
            if (stackPanel != null)
            {
                SetFontWeight(stackPanel, FontWeights.UltraBold);
            }
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            var stackPanel = sender as StackPanel;
            if (stackPanel != null)
            {
                SetFontWeight(stackPanel, FontWeights.Normal);
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Xử lý sự kiện click ở đây
            var stackPanel = sender as StackPanel;
            if (stackPanel != null)
            {
                var stackPanelName = stackPanel.Name;

                // Thực hiện các hành động tương ứng với sự kiện click (ví dụ: hiển thị nội dung, chuyển trang, ...)
                switch (stackPanelName)
                {
                    case "homePanel":
                        mainFrame.Navigate(new Uri("Views/HomePage.xaml", UriKind.Relative));
                        break;
                    case "searchPanel":
                        MessageBox.Show("Search clicked!");
                        break;
                    case "musicPanel":
                        mainFrame.Navigate(new Uri("Views/MusicPage.xaml", UriKind.Relative));
                        break;
                    case "videoPanel":
                        mainFrame.Navigate(new Uri("Views/VideoPage.xaml", UriKind.Relative));
                        break;
                    case "settingPanel":
                        MessageBox.Show("Setting clicked!");
                        break;
        
                }
            }
        }

        private void SetFontWeight(StackPanel stackPanel, FontWeight fontWeight)
        {
            var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
            var icon = stackPanel.Children.OfType<FontAwesome.WPF.FontAwesome>().FirstOrDefault();

            if (textBlock != null && icon != null)
            {
                textBlock.FontWeight = fontWeight;
                icon.FontWeight = fontWeight;
            }
        }

        private void PlayIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(PlayerMedia.IsPlaying)
            {
                PlayerMedia.PauseSong();
                ChangeStateIcon();
                timer.Stop();
            } else if(PlayerMedia.IsPaused)
            {
                PlayerMedia.ContinueSong();
                ChangeStateIcon();
                timer.Start();
            }
            else if(PlayerMedia.IsStopped)
            {
                PlayerMedia.PlaySong(PlayerMedia.player.URL);
                ChangeStateIcon();
                timer.Start();
            }
        }

        public void OnPlayPauseStateChanged(object sender, bool isPlaying)
        {
            if (isPlaying)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Pause;
                timer.Start();

            }
            else
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Play;
                timer.Stop();

            }
        }

        public void ChangeStateIcon()
        {
            if (PlayerMedia.IsPlaying)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Pause;
                SetCurrentSongInfo();
            }
            else if (PlayerMedia.IsPaused)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Play;
                SetCurrentSongInfo();
            }
            else if (PlayerMedia.IsStopped)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Play;
            }
        }

        public void SetCurrentSongInfo()
        {
            if(PlayerMedia.CurrentSong != null)
            {
                nameSong.Text = PlayerMedia.CurrentSong.Title;
                nameArtist.Text = PlayerMedia.CurrentSong.Artist;
                EndDurationInfoSong.Text = PlayerMedia.CurrentSong.Length;
            }
          
        }

        public void UpdateProcessingInfo()
        {
            if (PlayerMedia.CurrentSong != null && PlayerMedia.player != null )
            {

                int currentPosition = PlayerMedia.GetCurrentSongPosition();
               
                if(PlayerMedia.player.playState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    if(PlayerMedia.ShuffleMode)
                    {
                        PlayerMedia.RandomSong();
                    }
                    else
                    {
                        PlayerMedia.NextSong();
                    }
                    timer.Start();

                }

                if (currentPosition % 60 < 10)
                {
                    DurationInfoSong.Text = $"{currentPosition / 60}:0{(currentPosition % 60)}";
                }
                else
                {
                    DurationInfoSong.Text = $"{currentPosition / 60}:{(currentPosition % 60)}";
                }
                SliderBarProcessing.Maximum = PlayerMedia.GetCurrentSongDuration();

                SliderBarProcessing.Value = currentPosition;
                SetCurrentSongInfo();
                ChangeStateIcon();
            }
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            if (PlayerMedia.IsPlaying)
            {
                // Cập nhật thời gian liên tục
                UpdateProcessingInfo();
            } 
            
        }

        private bool seeking = false;
        private int lastPos = 0;
        private void SliderBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (seeking && PlayerMedia.CurrentSong != null)
            {
                int value = Convert.ToInt32(SliderBarProcessing.Value);
                if (Math.Abs(value - lastPos) > 1)
                {
                    PlayerMedia.SetSongPosition(value);
                    lastPos = value;
                }
            }
         
        }

        private void SliderBarProcessing_GotMouseCapture(object sender, MouseEventArgs e)
        {
            seeking = true;
           
        }

        private void SliderBarProcessing_LostMouseCapture(object sender, MouseEventArgs e)
        {
            seeking = false;
        }

        private void NextSongBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(PlayerMedia.CurrentSong != null) { 
                PlayerMedia.NextSong();
                timer.Start();

            }

        }

        private void PreSongBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PlayerMedia.CurrentSong != null)
            {
                PlayerMedia.PreviousSong();
                timer.Start();
            }
        }

        private void RandomSongBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (PlayerMedia.ShuffleMode)
            {
                PlayerMedia.SetShuffleMode(false);
                RandomSongBtn.Foreground = Brushes.White;
            }
            else
            {
                PlayerMedia.SetShuffleMode(true);
                RandomSongBtn.Foreground = Brushes.Green;
            }
        }

        private void RepeatSongBtn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(PlayerMedia.RepeatMode == RepeatMode.One)
            {
                PlayerMedia.SetRepeatMode(RepeatMode.Off);
                RepeatSongBtn.Foreground = Brushes.White;
            }
            else
            {
                PlayerMedia.SetRepeatMode(RepeatMode.One);
                RepeatSongBtn.Foreground = Brushes.Green;
            }
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            int value = Convert.ToInt32(SliderVolume.Value);
            if (Math.Abs(value - lastPos) > 1)
            {
                PlayerMedia.SetVolume(value);
                lastPos = value;
            }
        }

        private void SliderVolume_GotMouseCapture(object sender, MouseEventArgs e)
        {

        }

        private void SliderVolume_LostMouseCapture(object sender, MouseEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if(e.Key == Key.Space)
            {
               

            }   
            
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
  
            // select song and play
            RecentView? selected = ((FrameworkElement)sender).DataContext as RecentView;

            if (selected != null)
            {
                PlayerMedia.CurrentPlaylist = mainWindowVM.PlaylistService.GetPlaylist(selected.PlaylistId);
                if(PlayerMedia.CurrentPlaylist.Type == PlaylistType.Song)
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

                    if(CurrentSong != null)
                    {
                        PlayerMedia.CurrentSong = CurrentSong;
                        PlayerMedia.CurrentSongIndex = songIndex;
                        PlayerMedia.PlaySong(CurrentSong.Path);
                        timer.Start();
                    } else
                    {
                        // reload recents
                        mainWindowVM.LoadRecents();
                        recentsList.ItemsSource = mainWindowVM.RecentViews;
                        MessageBox.Show("File not found!");

                    }


                } else if(PlayerMedia.CurrentPlaylist.Type == PlaylistType.Video)
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

                    if(CurrentVideo != null)
                    {
                        PlayerMedia.CurrentVideo = CurrentVideo;
                        PlayerMedia.PauseSong();
                        var dialog = new VideoViewDialog();
                        bool? result = dialog.ShowDialog();
                    } else
                    {
                        // reload recents
                        mainWindowVM.LoadRecents();
                        recentsList.ItemsSource = mainWindowVM.RecentViews;
                        MessageBox.Show("File not found!");

                    }
                }
               
            }
            
        }
    }
}

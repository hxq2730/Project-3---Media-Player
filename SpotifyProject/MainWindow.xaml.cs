﻿using SpotifyProject.Models;
using SpotifyProject.Services;
using SpotifyProject.ViewModels;
using SpotifyProject.Views;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            mainFrame.Navigate(new HomePage(Bottom_Bar_Music));

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
                SolidColorBrush activeBrush = new SolidColorBrush(Colors.DeepSkyBlue);
                SolidColorBrush defaultBrush = new SolidColorBrush(Colors.White);


                // Thực hiện các hành động tương ứng với sự kiện click (ví dụ: hiển thị nội dung, chuyển trang, ...)
                switch (stackPanelName)
                {
                    case "homePanel":

                        homeText.Foreground = activeBrush;
                        homeIcon.Foreground = activeBrush;
                        musicText.Foreground = defaultBrush;
                        musicIcon.Foreground = defaultBrush;
                        videoText.Foreground = defaultBrush;
                        videoIcon.Foreground = defaultBrush;

                        Bottom_Bar_Music.Visibility = Visibility.Visible;
                        mainFrame.Navigate(new HomePage(Bottom_Bar_Music));
                        break;

                    case "musicPanel":
                        homeText.Foreground = defaultBrush;
                        homeIcon.Foreground = defaultBrush;
                        musicText.Foreground = activeBrush;
                        musicIcon.Foreground = activeBrush;
                        videoText.Foreground = defaultBrush;
                        videoIcon.Foreground = defaultBrush;


                        Bottom_Bar_Music.Visibility = Visibility.Visible;
                        mainFrame.Navigate(new MusicPage(Bottom_Bar_Music));
                        break;
                    case "videoPanel":
                        homeText.Foreground = defaultBrush;
                        homeIcon.Foreground = defaultBrush;
                        musicText.Foreground = defaultBrush;
                        musicIcon.Foreground = defaultBrush;
                        videoText.Foreground = activeBrush;
                        videoIcon.Foreground = activeBrush;
                        Bottom_Bar_Music.Visibility = Visibility.Visible;
                        mainFrame.NavigationService.Navigate(new VideoPage(Bottom_Bar_Music));
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
            if (PlayerMedia.IsPlaying)
            {
                PlayerMedia.PauseSong();
                ChangeStateIcon();
                timer.Stop();
            }
            else if (PlayerMedia.IsPaused)
            {
                PlayerMedia.ContinueSong();
                ChangeStateIcon();
                timer.Start();
            }
            else if (PlayerMedia.IsStopped)
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
            if (PlayerMedia.CurrentSong != null)
            {
                nameSong.Text = PlayerMedia.CurrentSong.Title;
                EndDurationInfoSong.Text = PlayerMedia.CurrentSong.Length;
            }

        }

        public void UpdateProcessingInfo()
        {
            if (PlayerMedia.CurrentSong != null && PlayerMedia.player != null)
            {

                int currentPosition = PlayerMedia.GetCurrentSongPosition();

                if (PlayerMedia.player.playState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    if (PlayerMedia.ShuffleMode)
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
            if (PlayerMedia.CurrentSong != null)
            {
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

            if (e.Key == Key.Space)
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
                        timer.Start();
                        Bottom_Bar_Music.Visibility = Visibility.Visible;
                        mainFrame.Navigate(new HomePage(Bottom_Bar_Music));
                    }
                    else
                    {
                        // reload recents
                        mainWindowVM.LoadRecents();
                        recentsList.ItemsSource = mainWindowVM.RecentViews;
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
                        mainFrame.Navigate(new ViewVideo());
                        Bottom_Bar_Music.Visibility = Visibility.Collapsed;


                    }
                    else
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

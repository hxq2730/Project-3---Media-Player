using SpotifyProject.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace SpotifyProject.Views
{
    /// <summary>
    /// Interaction logic for ViewVideo.xaml
    /// </summary>
    public partial class ViewVideo : Page
    {
        public ViewVideo()
        {
            InitializeComponent();
            //Closed += VideoViewDialog_Closed;
            //mediaElement.Close();
            //mediaElement.Play();
            // Dừng DispatcherTimer
            if (timer != null)
            {
                timer.Stop();
            }
        }

        private bool isSeeking = false;
        private DispatcherTimer timer;

        private void VideoViewDialog_Closed(object sender, EventArgs e)
        {
            // Tắt media khi cửa sổ được đóng
            mediaElement.Close();
            // Dừng DispatcherTimer
            if (timer != null)
            {
                timer.Stop();
            }
        }

        public void SetCurrentVideoInfo()
        {
            if (PlayerMedia.CurrentVideo != null)
            {
                nameSong.Text = PlayerMedia.CurrentVideo.Title;
                //nameArtist.Text = PlayerMedia.CurrentVideo.Artist;
                EndDurationInfoSong.Text = PlayerMedia.CurrentVideo.Length;
            }

        }
        private void videoScreen_Loaded(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri(PlayerMedia.CurrentVideo.Path);
            // Đặt LoadedBehavior và UnloadedBehavior là Manual
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            // Bắt đầu phát video
            mediaElement.Play();
            PlayerMedia.IsPlaying = true;
            PlayerMedia.IsPaused = false;
            PlayerMedia.IsStopped = false;
            ChangeStateIcon();

            // PlayerMedia.CurrentVideo = mediaElement;
            //PlayerMedia.CurrentSongIndex = songIndex;
            //PlayerMedia.PlaySong(CurrentSong.Path);
            SetCurrentVideoInfo();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Cập nhật mỗi giây
            timer.Tick += timeTick;
            timer.Start();
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Đặt giá trị tối đa cho thanh thời gian tua video
            timelineSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            // get maxium volume
            volumeSlider.Maximum = 1.0;

        }
        public void UpdateProcessingInfo()
        {
            //if (!isSeeking) return;
            if (PlayerMedia.CurrentVideo != null && PlayerMedia.player != null)
            {

                // int currentPosition = PlayerMedia.GetCurrentSongPosition();
                TimeSpan position = mediaElement.Position;
                int currentPosition = (int)position.TotalSeconds;



                if (currentPosition % 60 < 10)
                {
                    DurationInfoSong.Text = $"{currentPosition / 60}:0{(currentPosition % 60)}";
                }
                else
                {
                    DurationInfoSong.Text = $"{currentPosition / 60}:{(currentPosition % 60)}";
                }

                //timelineSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                timelineSlider.Value = currentPosition;
                // SetCurrentSongInfo();
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
                //isSeeking = true;
                ChangeStateIcon();
            }
        }

        private void timeTick(object? sender, EventArgs e)
        {
            if (PlayerMedia.IsPlaying)
            {
                // Cập nhật thời gian liên tục
                UpdateProcessingInfo();
            }

        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Tự động dừng khi video kết thúc
            mediaElement.Stop();
        }

        private void MediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // Xử lý khi phát video gặp lỗi
            MessageBox.Show($"Media failed to load: {e.ErrorException.Message}");
        }

        private void TimelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // if (isSeeking) return;
            if (mediaElement != null && mediaElement.NaturalDuration.HasTimeSpan && isSeeking)
            {
                TimeSpan newPosition = TimeSpan.FromSeconds(e.NewValue);
                mediaElement.Position = newPosition;
            }

        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement != null)
            {
                mediaElement.Volume = e.NewValue;
            }
        }

        private void SliderVolume_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void SliderVolume_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private int lastPos = 0;
        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = Convert.ToInt32(volumeSlider.Value);
            if (Math.Abs(value - lastPos) > 1)
            {
                PlayerMedia.SetVolume(value);
                lastPos = value;
            }
        }

        private void RandomSongBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void NextSongBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (PlayerMedia.CurrentVideo != null)
            {
                mediaElement.Stop();
                PlayerMedia.NextVideo();
                videoScreen_Loaded(sender, e);

            }
        }
        private void PreSongBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PlayerMedia.CurrentVideo != null)
            {
                mediaElement.Stop();
                PlayerMedia.PreviousVideo();
                videoScreen_Loaded(sender, e);

            }
        }

        public void ChangeStateIcon()
        {
            if (PlayerMedia.IsPlaying)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Pause;
                //SetCurrentSongInfo();
            }
            else if (PlayerMedia.IsPaused)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Play;
                // SetCurrentSongInfo();
            }
            else if (PlayerMedia.IsStopped)
            {
                PlayIcon.Icon = FontAwesome.WPF.FontAwesomeIcon.Play;
            }
        }

        private void PlayIcon_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            if (PlayerMedia.IsPlaying)
            {
                PlayerMedia.PauseSong();
                ChangeStateIcon();
                timer.Stop();
                mediaElement.Pause();
            }
            else if (PlayerMedia.IsPaused)
            {
                PlayerMedia.ContinueSong();
                ChangeStateIcon();
                timer.Start();
                mediaElement.Play();
            }
            else if (PlayerMedia.IsStopped)
            {
                PlayerMedia.PlaySong(PlayerMedia.player.URL);
                ChangeStateIcon();
                timer.Start();
                mediaElement.Play();
            }
        }



        private void RepeatSongBtn_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (PlayerMedia.RepeatMode == RepeatMode.One)
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

        private void SliderBarProcessing_GotMouseCapture(object sender, MouseEventArgs e)
        {
            isSeeking = true;

        }

        private void SliderBarProcessing_LostMouseCapture(object sender, MouseEventArgs e)
        {
            isSeeking = false;
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
                mediaElement.Stop();
            }
        }

        private void videoScreen_UnLoaded(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }
    }
}

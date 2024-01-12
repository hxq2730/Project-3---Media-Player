using SpotifyProject.Services;
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
using System.Windows.Threading;

namespace SpotifyProject.Views.Dialog
{
    /// <summary>
    /// Interaction logic for VideoViewDialog.xaml
    /// </summary>
    public partial class VideoViewDialog : Window
    {
        public VideoViewDialog()
        {
            InitializeComponent();
            Closed += VideoViewDialog_Closed;
        }
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
        private void videoScreen_Loaded(object sender, RoutedEventArgs e)
        {
            mediaElement.Source = new Uri(PlayerMedia.CurrentVideo.Path);
            // Đặt LoadedBehavior và UnloadedBehavior là Manual
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            // Bắt đầu phát video
            mediaElement.Play();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Cập nhật mỗi giây
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Cập nhật giá trị thanh Slider theo thời gian hiện tại của video
            if (mediaElement != null && mediaElement.NaturalDuration.HasTimeSpan)
            {
                timelineSlider.Value = mediaElement.Position.TotalSeconds;
            }
        }


        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

        private void VolumeUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Volume < 1.0)
            {
                mediaElement.Volume += 0.1;
            }
        }

        private void VolumeDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.Volume > 0.0)
            {
                mediaElement.Volume -= 0.1;
            }
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Đặt giá trị tối đa cho thanh thời gian tua video
            timelineSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            // get maxium volume
            volumeSlider.Maximum = 1.0;


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
            if (mediaElement != null && mediaElement.NaturalDuration.HasTimeSpan)
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
    }

}

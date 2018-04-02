using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace NaiveMediaPlayer1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            outputfile.Text = "正在播放文件：无";
        }

        private async void Search_TappedAsync(object sender, TappedRoutedEventArgs e)
        {
            //文件选择器
            var openPicker = new FileOpenPicker();

            //选择视图模式  
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List;  

            //初始位置  
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

            //添加文件类型  
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".mp4");

            StorageFile folder = await openPicker.PickSingleFileAsync();

            if (folder != null)
            {
                var mediaSource = MediaSource.CreateFromStorageFile(folder);
                player.Source = mediaSource.Uri;
                if (folder.FileType == ".mp3" || folder.FileType == ".mp4")
                {
                    outputfile.Text = "正在播放文件: " + folder.Name;
                    var stream = await folder.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    player.SetSource(stream, folder.ContentType);
                    player.Play();
                }
            }
        }
    }
}

/*
namespace orz
{

    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    //“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

    namespace Homework8
    {
        /// <summary>
        /// 可用于自身或导航至 Frame 内部的空白页。
        /// </summary>
        public sealed partial class MainPage : Page
        {
            MediaPlayer _mediaPlayer = new MediaPlayer();
            MediaTimelineController _mediaTimelineController = new MediaTimelineController();
            TimeSpan _duration;
            // MediaPlaybackSession _mediaPlaybackSession = new MediaPlaybackSession();
            public MainPage()
            {
                this.InitializeComponent();
                var mediaSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/Video1.MP4"));
                mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
                _mediaPlayer.Source = mediaSource;
                _mediaPlayer.CommandManager.IsEnabled = false;
                _mediaPlayer.TimelineController = _mediaTimelineController;
                //_mediaPlayer.Play();
                _mediaPlayerElement.SetMediaPlayer(_mediaPlayer);
            }

            private void pause_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
                    {
                        EllStoryboard.Pause();
                        _mediaTimelineController.Pause();
                    }
                    else
                    {
                        //EllStoryboard.Resume();
                        EllStoryboard.Begin();
                        _mediaTimelineController.Resume();
                    }
                }
                catch
                {

                }
            }

            private void start_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += timer_Tick;
                    timer.Start();
                    EllStoryboard.Begin();
                    _mediaTimelineController.Start();
                }
                catch
                {

                }

            }
            void timer_Tick(object sender, object e)
            {
                timeLine.Value = ((TimeSpan)_mediaTimelineController.Position).TotalSeconds;
                if (timeLine.Value == timeLine.Maximum)
                {
                    _mediaTimelineController.Position = TimeSpan.FromSeconds(0);
                    _mediaTimelineController.Pause();
                    EllStoryboard.Stop();
                }
            }
            private void stop_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    _mediaTimelineController.Position = TimeSpan.FromSeconds(0);
                    _mediaTimelineController.Pause();
                    EllStoryboard.Stop();
                }
                catch
                {

                }

            }

            private void display_Click(object sender, RoutedEventArgs e)
            {
                ApplicationView view = ApplicationView.GetForCurrentView();
                bool isInFullScreenMode = view.IsFullScreenMode;
                if (isInFullScreenMode)
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/3.jpg", UriKind.Absolute));
                    MyGrid.Background = imageBrush;
                    MyGrid.Background.Opacity = 0.5;
                    view.ExitFullScreenMode();
                }
                else
                {
                    MyGrid.Background = new SolidColorBrush(Colors.Black);// Windows.UI.Xaml.Media.Brush
                    view.TryEnterFullScreenMode();
                }
            }

            private async void add_Click(object sender, RoutedEventArgs e)
            {
                var openPicker = new FileOpenPicker();

                openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
                openPicker.FileTypeFilter.Add(".wmv");
                openPicker.FileTypeFilter.Add(".mp4");
                openPicker.FileTypeFilter.Add(".mp3");
                openPicker.FileTypeFilter.Add(".wma");

                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    var mediaSource = MediaSource.CreateFromStorageFile(file);
                    mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
                    _mediaPlayer.Source = mediaSource;
                    if (file.FileType == ".mp3" || file.FileType == ".wma")
                    {
                        Picture.Visibility = Visibility.Visible;
                        //_mediaPlayerElement.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Picture.Visibility = Visibility.Collapsed;
                        //_mediaPlayerElement.Visibility = Visibility.Visible;
                    }
                }

            }

            private void Volumn_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
            {
                _mediaPlayer.Volume = (double)Volumn.Value;
            }

            private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
            {
                _duration = sender.Duration.GetValueOrDefault();

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    timeLine.Minimum = 0;
                    timeLine.Maximum = _duration.TotalSeconds;
                    timeLine.StepFrequency = 1;
                });
            }
        }
    }
}
*/
using System;
using System.Collections.Generic;
using System.Text;

#if ANDROID
using Android.Media;
using OtomeJanai.Android.Common;
#elif WINDOWS_UWP
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FFmpegInterop;
using Windows.Storage.Streams;
using System.IO;

#elif LINUX

#else

#endif

namespace OtomeJanai.Shared.Common
{
    internal class SoundEngine
    {

#if ANDROID

#elif WINDOWS_UWP

#elif LINUX

#else

#endif

        #region Private member
#if ANDROID
        private MediaPlayer _player;
#elif WINDOWS_UWP
        private MediaElement _player;
        private IRandomAccessStream _fileStream;
        private FFmpegInteropMSS _ffmpegMSS;
#elif LINUX

#else

#endif

        #endregion


        public bool IsPlaying
        {
            get
            {
#if ANDROID
                return _player.IsPlaying;
#elif WINDOWS_UWP
                return _player.CurrentState == MediaElementState.Playing;
#elif LINUX

#else

#endif
            }
        }

        public bool IsLoop
        {
            get
            {
#if ANDROID
                return _player.Looping;
#elif WINDOWS_UWP
                return _player.IsLooping;
#elif LINUX

#else

#endif
            }
            internal set
            {
#if ANDROID
                _player.Looping = value;
#elif WINDOWS_UWP
                _player.IsLooping = value;
#elif LINUX

#else

#endif
            }
        }

        public SoundEngine()
        {
#if ANDROID
            _player = new MediaPlayer();
            _player.SetAudioStreamType(Stream.Music);
#elif WINDOWS_UWP
            _player = new MediaElement();
#elif LINUX
            
#else
            
#endif
        }


        internal void PlayFromStream(System.IO.Stream data)
        {
#if WINDOWS_UWP
            _player.Stop();
            _fileStream?.Dispose();
            _fileStream = null;
            _ffmpegMSS?.Dispose();
            _ffmpegMSS = null;
            var mems = new MemoryStream();
            data.CopyTo(mems);
            _fileStream = mems.AsRandomAccessStream();
            _ffmpegMSS = FFmpegInteropMSS.CreateFFmpegInteropMSSFromStream(_fileStream, false, false);
            _player.SetMediaStreamSource(_ffmpegMSS.GetMediaStreamSource());
            _player.Play();
#elif ANDROID
            _player.Stop();
            _player.Reset();
            //TODO: StreamMediaDataSource required Android 6.0, lower the requirement
            _player.SetDataSource(new StreamMediaDataSource(data));
            _player.Prepare();
            _player.Start();
#elif LINUX

#else

#endif
        }

        internal void Pause()
        {
#if ANDROID || WINDOWS_UWP
            _player.Pause();
#elif LINUX

#else

#endif
        }

        internal void Resume()
        {

#if ANDROID
            _player.Start();
#elif WINDOWS_UWP
            _player.Play();
#elif LINUX

#else

#endif
        }

    }
}

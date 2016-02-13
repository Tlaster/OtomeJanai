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
using System.IO;
using OtomeJanai.Linux.Common;
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore;
#else
using CSCore.Codecs;
using CSCore.SoundOut;
using CSCore;
using OtomeJanai.Win32.Common;
using System.IO;
#endif

namespace OtomeJanai.Shared.Common
{
    internal class SoundEngine
    {
        #region Private member
#if ANDROID
        private MediaPlayer _player;
        private float _volume;
#elif WINDOWS_UWP
        private MediaElement _player;
        private IRandomAccessStream _fileStream;
        private FFmpegInteropMSS _ffmpegMSS;
#else
        private WasapiOut _soundOut;
        private IWaveSource _source;
        private bool _isLoop;
#endif

        #endregion

        public float Volume
        {
            get
            {
#if ANDROID
                return _volume;
#elif WINDOWS_UWP
                return Convert.ToSingle(_player.Volume);
#else
                return _soundOut.Volume;
#endif
            }
            internal set
            {
#if ANDROID
                _volume = value;
                _player.SetVolume(value, value);
#elif WINDOWS_UWP
                _player.Volume = value;
#else
                _soundOut.Volume = value;
#endif
            }
        }

        public bool IsPlaying
        {
            get
            {
#if ANDROID
                return _player.IsPlaying;
#elif WINDOWS_UWP
                return _player.CurrentState == MediaElementState.Playing;
#else
                return _soundOut.PlaybackState == PlaybackState.Playing;
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
#else
                return _isLoop;
#endif
            }
            internal set
            {
#if ANDROID
                _player.Looping = value;
#elif WINDOWS_UWP
                _player.IsLooping = value;
#else
                _isLoop = value;
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
#else
            CodecFactory.Instance.Register("ogg", new CodecFactoryEntry(s => new NVorbisSource(s).ToWaveSource(), ".ogg"));
            _soundOut = new WasapiOut();
            _soundOut.Stopped += _soundOut_Stopped;
#endif
        }
#if WINDOWS ||LINUX
        private void _soundOut_Stopped(object sender, PlaybackStoppedEventArgs e)
        {
            if (IsLoop)
            {
                lock (_source)
                {
                    lock (_soundOut)
                    {
                        _source.SetPosition(TimeSpan.FromMinutes(0));
                        _soundOut.Play();
                    }
                }
            }
        }
#endif

        internal async void PlayFromStream(System.IO.Stream data, string fileName)
        {
#if WINDOWS_UWP
            _player.Stop();
            _fileStream?.Dispose();
            _fileStream = null;
            _ffmpegMSS?.Dispose();
            _ffmpegMSS = null;
            var mems = new MemoryStream();
            await data.CopyToAsync(mems);
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
#else
            _source?.Dispose();
            _source = null;
            var mems = new MemoryStream();
            await data.CopyToAsync(mems);
            _source = CodecFactory.Instance.GetCodec(fileName.ToLower(), mems);
            _soundOut.Initialize(_source);
            _soundOut.Play();
#endif
        }

        internal void Pause()
        {
#if ANDROID || WINDOWS_UWP
            _player.Pause();
#else
            if (_soundOut.PlaybackState == PlaybackState.Playing)
                _soundOut.Pause();
#endif
        }

        internal void Resume()
        {

#if ANDROID
            _player.Start();
#elif WINDOWS_UWP
            _player.Play();
#else
            if (_soundOut.PlaybackState == PlaybackState.Paused)
                _soundOut.Resume();
#endif
        }

    }
}

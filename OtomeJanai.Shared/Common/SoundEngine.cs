using System;
using System.Collections.Generic;
using System.Text;

#if ANDROID
using Android.Media;
using OtomeJanai.Android.Common;
#elif WINDOWS_UWP

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

#elif LINUX

#else

#endif
            }
            internal set
            {
#if ANDROID
                _player.Looping = value;
#elif WINDOWS_UWP

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

#elif LINUX

#else

#endif
        }


        internal void PlayFromBytes(byte[] data)
        {
#if ANDROID
            _player.Reset();
            //TODO: BytesMediaDataSource required Android 6.0, lower the requirement
            _player.SetDataSource(new BytesMediaDataSource(data));
            _player.Prepare();
            _player.Start();
#elif WINDOWS_UWP
            
#elif LINUX

#else

#endif
        }

        internal void Pause()
        {
#if ANDROID
            _player.Pause();
#elif WINDOWS_UWP

#elif LINUX

#else

#endif
        }

        internal void Resume()
        {

#if ANDROID
            _player.Start();
#elif WINDOWS_UWP

#elif LINUX

#else

#endif
        }

    }
}

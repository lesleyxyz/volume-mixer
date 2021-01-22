using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace VolumeMixer
{
    class MasterAudioSession : IAudioSession
    {
        public float Volume { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        // TODO: set/get volume of currently running game

        public ImageSource imageSource {
            get => null;
        }

        override public String ToString()
        {
            return "Master volume";
        }
    }
}

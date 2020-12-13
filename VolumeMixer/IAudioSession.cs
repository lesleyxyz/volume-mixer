using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace VolumeMixer
{
    interface IAudioSession
    {
        float Volume { get; set; }
        ImageSource imageSource { get; }
        string ToString();
    }
}

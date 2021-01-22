using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace VolumeMixer
{
    class VMPacket
    {
        public bool isPing { get; set; }
        public int knobId;
        public int value;

        public VMPacket(byte[] packet)
        {
            isPing = (packet[3] == 0x00); //TODO: throw error if also not 0xaa?
            knobId = (isPing) ? -1 : packet[4];
            value = (isPing) ? -1 :  packet[6];

            if (!isPing)
            {
                byte check = (byte)(packet[4] ^ packet[6]);
                if (check != packet[5])
                {
                    Debug.WriteLine("CHECK FAILED"); //TODO: throw error, resync in PacketManager
                    isPing = true;
                }
            }
        }
    }
}

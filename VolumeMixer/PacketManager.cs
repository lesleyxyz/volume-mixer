using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;

namespace VolumeMixer
{
    class PacketManager
    {
        private SerialPort _serialPort;
        private Dispatcher _dispatcher;
        private bool _continue = true;
        private List<DialViewHolder> _dialViewHolders;

        public PacketManager(SerialPort serialPort, List<DialViewHolder> dialViewHolders, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _dialViewHolders = dialViewHolders;
            _serialPort = serialPort;
            if (!serialPort.IsOpen)
            { //TODO: throw error
                serialPort.Open();
            }
            Thread readThread = new Thread(Read);
            readThread.Start();
        }

        private void Read()
        {
            while (_continue)
            {
                try
                {
                    if (_serialPort.BytesToRead < 8)
                        continue;

                    byte[] buffer = new byte[8];
                    _serialPort.Read(buffer, 0, 8);

                    VMPacket packet = new VMPacket(buffer);

                    if (packet.isPing || packet.knobId - 1 < 0 || packet.knobId - 1 >= _dialViewHolders.Count)
                        continue;

                    _dispatcher.Invoke(() =>
                    {
                        try
                        {
                            Debug.WriteLineIf(packet.knobId == 1, packet.value);
                            DialViewHolder view = _dialViewHolders[packet.knobId - 1];
                            view.progressbar.Value = packet.value;
                            IAudioSession a = (IAudioSession)view.combobox.SelectedItem;

                            if (a == null)
                                return;

                            Debug.WriteLineIf(packet.knobId == 1, a.ToString());
                            a.Volume = (float)packet.value / (float)127;
                            Debug.WriteLineIf(packet.knobId == 1, a.Volume.ToString() + " " + packet.value);
                        }
                        catch (Exception)
                        {
                            
                        }
                    });
                }
                catch (IOException e) {
                    MessageBox.Show(e.Message);
                    MessageBox.Show(e.InnerException.Message);
                }
            }
        }
    }
}

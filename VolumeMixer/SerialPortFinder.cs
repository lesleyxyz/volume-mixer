using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Runtime.Serialization;
using System.Windows;

namespace VolumeMixer
{
    class SerialPortFinder
    {
        private static readonly byte[] pingPacket = { 0x1e, 0x51, 0xe4, 0x00, 0xFF, 0x88, 0xE2, 0x0a };

        public static SerialPort FindCorrectPortname()
        {
            foreach (String port in SerialPort.GetPortNames())
            {
                Debug.WriteLine("Trying " + port);
                SerialPort testPort = new SerialPort(port);

                testPort.BaudRate = 9600;
                testPort.Parity = Parity.None;
                testPort.Handshake = Handshake.None;
                testPort.NewLine = "\n";
                testPort.RtsEnable = false;
                testPort.ReadTimeout = 1000;
                testPort.WriteTimeout = 500;
                testPort.ReadBufferSize = 1048576;

                try
                {
                    testPort.Open();
                    System.Threading.Thread.Sleep(200);
                    int bufferSize = 8;
                    byte[] buffer = new byte[bufferSize];
                    testPort.Read(buffer, 0, bufferSize); //Get initial buffer
                    Debug.WriteLine(BitConverter.ToString(buffer));
                    int offset = calculateOffset(buffer, bufferSize); // We might started listening while the device was writing to the Serial port, we have to know where in the packet the device was
                    if (isValidPacket(buffer, offset))
                    {
                        Debug.WriteLine("found port " + port);
                        return testPort;
                    }
                }
                catch (IOException) { }
                catch (TimeoutException) { }
                catch (InvalidPacketException) { }

                if (testPort.IsOpen)
                    testPort.Close();

                testPort.Dispose();

                continue;
            }
            
            return FindCorrectPortname();//return null;
        }

        static int calculateOffset(byte[] buffer, int bufferSize)
        {
            for (int i = 0; i < bufferSize; i++)
                if(buffer[i] == pingPacket[0]) // Start byte of VolumeMixer packet
                    return i;

            throw new InvalidPacketException("Buffer does not contain magic byte");
        }

        // Check if first 3 bytes (header) match
        static bool isValidPacket(byte[] buffer, int offset)
        {
            int toCheck = 3;

            for (int i = 0; i < toCheck; i++)
                if(buffer[(i + offset) % toCheck] != pingPacket[i])
                    return false;

            return true;
        }
    }

    [Serializable]
    internal class InvalidPacketException : Exception
    {
        public InvalidPacketException()
        {
        }

        public InvalidPacketException(string message) : base(message)
        {
        }

        public InvalidPacketException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPacketException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

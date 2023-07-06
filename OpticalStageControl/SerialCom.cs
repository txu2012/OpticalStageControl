using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace OpticalStageControl
{
    class SerialException : Exception
    {
        public SerialException() {}
        public SerialException(string message) : base(message) {}
        public SerialException(string message, Exception inner) : base(message, inner) {}

        private string strExtraInfo;
        public string ExtraErrorInfo
        {
            get
            {
                return strExtraInfo;
            }

            set
            {
                strExtraInfo = value;
            }
        }
    }

    public class SerialCom
    {
        private SerialPort port;
        public string PortName { get; set; }
        private int baud = 115200;
        public static string[] PortList { get { return SerialPort.GetPortNames(); } }
        public bool PortConnected { get; set; }

        public SerialCom(string port_name) 
        {
            PortName = port_name;
            this.port = new SerialPort(PortName, 115200, Parity.None, 8);
            PortConnected = false;
        }

        public void OpenPort()
        {
            port.Open();
            port.Encoding = System.Text.Encoding.GetEncoding(28591);
            port.WriteTimeout = 2000;
            port.ReadTimeout = 2000;
            Thread.Sleep(1000);

            if (port.IsOpen)
                PortConnected = true;
            
            if (!PortConnected)
                ClosePort();
        }

        public void ClosePort()
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
                port = null;
                PortConnected = false;
            }
        }

        public byte[] PortWriteReadByte(byte[] byte_arr, int length, int expected_response_len = 999)
        {
            byte[] data = new byte[1];
            try
            {
                if (!port.IsOpen)
                    throw new SerialException("Device is not connected.");
                Console.WriteLine($"byteArr:{BitConverter.ToString(byte_arr)}, length:{byte_arr.Length}");
                port.Write(byte_arr, 0, length);

                Thread.Sleep(100);
                int buffer = port.ReadByte();

                Console.WriteLine($"buffer:{buffer}");
                if (buffer > 0)
                {
                    data = new byte[buffer];
                    port.Read(data, 0, buffer);

                    Console.WriteLine($"data:{BitConverter.ToString(data)}");
                }

                if (expected_response_len != 999)
                {
                    if (buffer != expected_response_len)
                        throw new SerialException("Response different from expected");
                }

                return data;
            }
            catch(SerialException ex)
            {
                Console.WriteLine(ex.Message);
                return data;
            }
            catch(TimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                return new byte[] { 0xff, 0xfe };
            }
        }

        public List<byte[]> TestCommand()
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x01, 0x01 });
            ret.Add(PortWriteReadByte(ret[0], 2, 1));
            PrintByteArr(ret[1]);
            
            return ret;
        }

        public byte QueryFirmware()
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x01, 0x02 });
            ret.Add(PortWriteReadByte(ret[0], 2, 2));
            PrintByteArr(ret[1]);

            return ret[1][1];
        }

        private void PrintByteArr(byte[] arr)
        {
            string hex = BitConverter.ToString(arr);
            Console.WriteLine(hex);
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        public static byte[] Combine(byte[] first, byte[] second, byte[] third)
        {
            byte[] ret = new byte[first.Length + second.Length + third.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            Buffer.BlockCopy(third, 0, ret, first.Length + second.Length, third.Length);

            return ret;
        }
    }
}

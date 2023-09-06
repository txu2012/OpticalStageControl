using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;

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
        public static string[] PortList { get { return SerialPort.GetPortNames(); } }
        public bool PortConnected { get; set; }

        public SerialCom(string port_name) 
        {
            PortName = port_name;
            this.port = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One);
            PortConnected = false;
        }

        public void OpenPort()
        {
            try
            {
                port.Open();
                port.WriteTimeout = 200;
                port.ReadTimeout = 200;

                if (port.IsOpen)
                    PortConnected = true;

                if (!PortConnected)
                    ClosePort();
            }
            catch(Exception ex)
            {
                ClosePort();
                Debug.WriteLine("Failed to open.");
            }
        }

        public void ClosePort()
        {
            try
            {
                if (port != null && port.IsOpen)
                {
                    port.Close();
                    port = null;
                    PortConnected = false;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Device may have been disconnected.");
            }
        }
        public void SetTimeout(double timeout)
        {
            if ( port.IsOpen)
                port.ReadTimeout = (int)timeout;
        }

        /***
         * Parameters:
         * @byte_arr              : Command list with parameters sent to the microcontroller
         * @expected_response_len : Length of the expected response from microcontroller, 
         *                          default set to 999 for testing serial commands to see output.
         * 
         * @return                : Array of bytes returned from the microcontroller. 
         *                          Checks first byte as the buffer for the next set of bytes for the response
         */
        public byte[] PortWriteReadByte(byte[] byte_arr, int expected_response_len = 999, int timeout_ms = 200)
        {
            try
            {
                if (!port.IsOpen)
                    throw new SerialException("Device is not connected.");

                int orig_timeout = port.ReadTimeout;
                port.ReadTimeout = timeout_ms;
                port.Write(byte_arr, 0, byte_arr.Length);

                List<byte> bytesRead = new List<byte>();
                int buffer = port.ReadByte();
                while (bytesRead.Count < buffer)
                {
                    bytesRead.Add((byte)port.ReadByte());
                }

                if (expected_response_len != 999)
                {
                    if (buffer != expected_response_len)
                        throw new SerialException($"Response different from expected. Expected bytes: {expected_response_len}, received: {buffer}");
                }

                return bytesRead.ToArray();
            }
            catch(SerialException ex)
            {
                Debug.WriteLine(ex.Message);
                return new byte[] { 0xff, 0xfe };
            }
            catch(TimeoutException ex)
            {
                Debug.WriteLine(ex.Message);
                return new byte[] { 0xff, 0xfe };
            }
        }

        public List<byte[]> TestCommand()
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x01, 0x01 });
            ret.Add(PortWriteReadByte(ret[0], 1));

            return ret;
        }

        public byte QueryFirmware()
        {
            byte[] firmware = PortWriteReadByte(new byte[] { 0x01, 0x02 }, 2);

            return firmware[1];
        }

        public byte[] QueryBoardName()
        {
            byte[] boardName = PortWriteReadByte(new byte[] { 0x01, 0x03 }, 17);
            string utfString = Encoding.UTF8.GetString(boardName, 1, boardName.Length-1);

            return boardName;
        }

        #region Append Byte Arrays
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
        #endregion
    }
}

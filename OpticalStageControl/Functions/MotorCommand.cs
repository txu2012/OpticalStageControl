using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OpticalStageControl
{
    public class MotorCommand
    {
        private SerialCom serialCom;
        public MotorCommand(SerialCom serial) { this.serialCom = serial; }

        public List<byte[]> MoveMotor(int motor_index, short position, double timeout_ms = 200)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(SerialCom.Combine(new byte[] { 0x04, 0x21, (byte)motor_index }, BitConverter.GetBytes(position)));
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3, (int)timeout_ms));
            return ret;
        }
        public void MoveMotorJs(int motor_index, short position, double timeout_ms = 200)
        {
            serialCom.PortWriteReadByte(SerialCom.Combine(new byte[] { 0x04, 0x41, (byte)motor_index }, BitConverter.GetBytes(position)), 1, (int)timeout_ms);
        }
        public List<byte[]> HomeMotor(int motor_index, bool center, double timeout_ms = 200)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x03, 0x20, (byte)motor_index, (byte)(center == true ? 0x01 : 0x00) });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3, (int)timeout_ms));
            return ret;
        }

        public List<byte[]> QueryPosition(int motor_index)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x02, 0x22, (byte)motor_index });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));
            return ret;
        }

        public List<byte[]> QueryLimit(int motor_index)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x02, 0x23, (byte)motor_index });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));
            return ret;
        }

        public List<byte[]> QueryVelociy()
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x01, 0x25 });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));
            return ret;
        }

        public List<byte[]> SetMode(int mode)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x02, 0x30, (byte)mode });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 1));
            return ret;
        }

        public List<byte[]> SetPosition(int motor_index, short motor_position)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(SerialCom.Combine(new byte[] { 0x04, 0x24, (byte)motor_index }, BitConverter.GetBytes(motor_position)));
            ret.Add(serialCom.PortWriteReadByte(ret[0], 1));
            return ret;
        }

        public List<byte[]> MeasureLimit()
        {
            int timeout_ms = (int)(((2000 / 100) * 1e3) + 500) * 3;
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x01, 0x26 });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3, timeout_ms));
            return ret;
        }
    }
}

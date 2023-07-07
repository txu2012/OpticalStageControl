using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpticalStageControl
{
    public class MotorCommand
    {
        private SerialCom serialCom;
        public MotorCommand(SerialCom serial) { this.serialCom = serial; }

        public List<byte[]> MoveMotor(int motor_index, short position)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(SerialCom.Combine(new byte[] { 0x04, 0x21, (byte)motor_index }, BitConverter.GetBytes(position)));
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));

            //short.TryParse(final_position, out MotorPosition[motorIndex]);
            Console.WriteLine(BitConverter.ToString(ret[1]));
            return ret;
        }

        public List<byte[]> QueryPosition(int motor_index)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x02, 0x22, (byte)motor_index });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));

            Console.WriteLine(BitConverter.ToString(ret[1]));
            return ret;
        }

        public List<byte[]> QueryLimit(int motor_index)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x02, 0x23, (byte)motor_index });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));

            Console.WriteLine(BitConverter.ToString(ret[1]));
            return ret;
        }

        public List<byte[]> QueryVelociy()
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x01, 0x25 });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 3));

            Console.WriteLine(BitConverter.ToString(ret[1]));
            return ret;
        }

        public List<byte[]> SetMode(int mode)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(new byte[] { 0x02, 0x30, (byte)mode });
            ret.Add(serialCom.PortWriteReadByte(ret[0], 1));

            Console.WriteLine(BitConverter.ToString(ret[1]));
            return ret;
        }

        public List<byte[]> SetPosition(int motor_index, short motor_position)
        {
            List<byte[]> ret = new List<byte[]>();
            ret.Add(SerialCom.Combine(new byte[] { 0x04, 0x24, (byte)motor_index }, BitConverter.GetBytes(motor_position)));
            ret.Add(serialCom.PortWriteReadByte(ret[0], 1));

            Console.WriteLine(BitConverter.ToString(ret[1]));
            return ret;
        }
    }
}

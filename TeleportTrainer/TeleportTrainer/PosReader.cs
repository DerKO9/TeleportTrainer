using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TeleportTrainer
{
    public static class PosReader
    {
        public static Point ReadCurrentPos()
        {
            Point point = new Point();
            Process process = null;
            try
            {
                process = Process.GetProcessesByName("YLILWin64")[0]; //"YLILWin64" "YookaLaylee64"
            }
            catch (Exception e)
            {
                throw new Exception("YookaLayleeIL process could not be found.");
            }
            MultiPointer xPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA0 });
            MultiPointer yPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA4 });
            MultiPointer zPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA8 });
            try
            {
                point.X = xPosPointer.ReadValue();
                point.Y = yPosPointer.ReadValue();
                point.Z = zPosPointer.ReadValue();
            }
            catch (Exception e)
            {
                throw new Exception("Could not read XYZ addresses.");
            }
            return point;
        }

        public static void WriteCurrentPos(Point point)
        {
            Process process = null;
            try
            {
                process = Process.GetProcessesByName("YLILWin64")[0]; //"YLILWin64" "YookaLaylee64"
            }
            catch (Exception e)
            {
                //MessageBox.Show("YookaLaylee process couldn't be found\n\n" + e.ToString());
                return;
            }
            MultiPointer xPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA0 });
            MultiPointer yPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA4 });
            MultiPointer zPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA8 });
            try
            {
                xPosPointer.WriteValue(point.X);
                yPosPointer.WriteValue(point.Y);
                zPosPointer.WriteValue(point.Z);
            }
            catch (Exception e)
            {
                //MessageBox.Show("The addresses could no be read\n\n" + e.ToString());
                return;
            }
        }
    }
}

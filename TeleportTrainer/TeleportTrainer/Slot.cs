using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace TeleportTrainer
{
    public sealed class Slot : INotifyPropertyChanged
    {
        //private Process process;
        private float _xPos;
        private float _yPos;
        private float _zPos;

        public float XPos
        {
            get => _xPos;
            set
            {
                _xPos = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(XPos)));
            }
        }
        public float YPos
        {
            get => _yPos;
            set
            {
                _yPos = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(YPos)));
            }
        }
        public float ZPos
        {
            get => _zPos;
            set
            {
                _zPos = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ZPos)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Set()
        {
            Process process = null;
            try
            {
                process = Process.GetProcessesByName("YLILWin64")[0]; //"YLILWin64" "YookaLaylee64"
            }
            catch (Exception e)
            {
                MessageBox.Show("YookaLaylee process couldn't be found\n\n" + e.ToString());
                return;
            }
            MultiPointer xPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA0 });
            MultiPointer yPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA4 });
            MultiPointer zPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA8 });
            try
            {
                XPos = xPosPointer.ReadValue();
                YPos = yPosPointer.ReadValue();
                ZPos = zPosPointer.ReadValue();
            }
            catch (Exception e)
            {
                MessageBox.Show("The addresses could no be read\n\n" + e.ToString());
                return;
            }
            
            
        }

        public void Recall()
        {
            Process process = null;
            try
            {
                process = Process.GetProcessesByName("YLILWin64")[0]; //"YLILWin64" "YookaLaylee64"
            }
            catch (Exception e)
            {
                MessageBox.Show("YookaLaylee process couldn't be found\n\n" + e.ToString());
                return;
            }
            MultiPointer xPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA0 });
            MultiPointer yPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA4 });
            MultiPointer zPosPointer = new MultiPointer(process, "UnityPlayer.dll", 0x0144DD68, new long[] { 0x128, 0x18, 0x10, 0xA8 });
            try
            {
                xPosPointer.WriteValue(XPos);
                yPosPointer.WriteValue(YPos);
                zPosPointer.WriteValue(ZPos);
            }
            catch (Exception e)
            {
                MessageBox.Show("The addresses could no be read\n\n" + e.ToString());
                return;
            }
        }

        public void SetRecall(float x, float y, float z)
        {
            XPos = x;
            YPos = y;
            ZPos = z;
            Recall();
        }
    }
}

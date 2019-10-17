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
        private Point position = new Point();

        public Point Position
        {
            get => position;
            set
            {
                position = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Position)));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void SaveCurrentPos()
        {
            Position = PosReader.ReadCurrentPos() ?? Position;
        }

        public void RecallSavedPos()
        {
            PosReader.WriteCurrentPos(Position);
        }

        public void SetPos(float x, float y, float z)
        {
            Position.X = x;
            Position.Y = y;
            Position.Z = z;
        }

        public void SetRecall(float x, float y, float z)
        {
            SetPos(x, y, z);
            RecallSavedPos();
        }
    }
}

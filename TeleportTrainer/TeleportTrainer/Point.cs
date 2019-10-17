using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleportTrainer
{
    public class Point
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}

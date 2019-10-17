using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TeleportTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private KeyboardHook _keyboardHook;
        private GamepadHook _gamepadHook;
        private string _currentPosDisplay;

        public string CurrentPosDisplay
        {
            get => _currentPosDisplay;
            set
            {
                _currentPosDisplay = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentPosDisplay)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Slot Slot1 { get; } = new Slot();
        public Slot Slot2 { get; } = new Slot();
        public Slot Slot3 { get; } = new Slot();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _gamepadHook = new GamepadHook();
            _gamepadHook.ButtonPressed += ControllerButtonPressed;

            _keyboardHook = new KeyboardHook();
            _keyboardHook.RegisterHotKey(System.Windows.Forms.Keys.NumPad1);
            _keyboardHook.RegisterHotKey(System.Windows.Forms.Keys.NumPad3);
            _keyboardHook.RegisterHotKey(System.Windows.Forms.Keys.NumPad4);
            _keyboardHook.RegisterHotKey(System.Windows.Forms.Keys.NumPad6);
            _keyboardHook.KeyPressed += KeyBoardKeyPressed;

            StartPollingInput();
            StartPollingCurrentPos();
        }

        private async void StartPollingInput()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(25);
                    _gamepadHook.Poll();
                    _keyboardHook.Poll();
                }
            });
        }

        private async void StartPollingCurrentPos()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    try
                    {
                        Point point = PosReader.ReadCurrentPos();
                        CurrentPosDisplay = $"Current Position X: {point.X}   Y: {point.Y}   Z: {point.Z}";
                    }
                    catch 
                    {
                        CurrentPosDisplay = $"Can't read position";
                    }
                }
            });
        }

        private void KeyBoardKeyPressed(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode.ToString());
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad4)
                Slot1.SaveCurrentPos();
            else if (e.KeyCode == System.Windows.Forms.Keys.NumPad6)
                Slot1.RecallSavedPos();
            else if (e.KeyCode == System.Windows.Forms.Keys.NumPad1)
                Slot2.SaveCurrentPos();
            else if (e.KeyCode == System.Windows.Forms.Keys.NumPad3)
                Slot2.RecallSavedPos();
        }

        private void ControllerButtonPressed(object sender, GamepadButton e)
        {
            Console.WriteLine(e.Button.ToString());
            if (e.Button.ToString() == "RotationX+")
                Slot1.SaveCurrentPos();
            else if (e.Button.ToString() == "RotationX-")
            {
                Slot1.RecallSavedPos();
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button)?.Name)
            {
                case "_GoButton1":
                    Slot1.RecallSavedPos();
                    break;
                case "_GoButton2":
                    Slot2.RecallSavedPos();
                    break;
            }
        }

        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button)?.Name)
            {
                case nameof(_tutorialEndButton):
                    Slot1.SetRecall(560, 1, 0);
                    break;
                case nameof(_Part1Start):
                    Slot1.SetRecall(-133, -80, 0);
                    break;
                case nameof(_Part2Start):
                    Slot1.SetRecall(-96, -2, 0);
                    break;
                case nameof(_Part3Start):
                    Slot1.SetRecall(-144, 0, 0);
                    break;
                case nameof(_Part4Start):
                    Slot1.SetRecall(-5, 4, 0);
                    break;
                case nameof(_BossStart):
                    Slot1.SetRecall(-284, 2, 0);
                    break;
                case nameof(_Part1End):
                    Slot1.SetRecall(510, 62, 0);
                    break;
                case nameof(_Part2End):
                    Slot1.SetRecall(506, 428, 0);
                    break;
                case nameof(_Part3End):
                    Slot1.SetRecall(144, 71, 0);
                    break;
                case nameof(_Part4End):
                    Slot1.SetRecall(0, 0, 0);
                    break;
                default:
                    break;
            }
        }
    }
}

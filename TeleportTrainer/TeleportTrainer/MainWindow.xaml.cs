using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
        private KeyboardHook _keyboardHook;
        private GamepadHook _gamepadHook;

        public Slot Slot1 { get; } = new Slot();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _gamepadHook = new GamepadHook();
            _gamepadHook.ButtonPressed += ControllerButtonPressed;

            _keyboardHook = new KeyboardHook();
            _keyboardHook.RegisterHotKey(System.Windows.Forms.Keys.NumPad1);
            _keyboardHook.RegisterHotKey(System.Windows.Forms.Keys.NumPad3);
            _keyboardHook.KeyPressed += KeyBoardKeyPressed;

            StartPolling();
        }

        private async void StartPolling()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    _gamepadHook.Poll();
                    _keyboardHook.Poll();
                }
            });
        }

        private void KeyBoardKeyPressed(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode.ToString());
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad1)
                Slot1.Set();
            else if (e.KeyCode == System.Windows.Forms.Keys.NumPad3)
                Slot1.Recall();
        }

        private void ControllerButtonPressed(object sender, GamepadButton e)
        {
            Console.WriteLine(e.Button.ToString());
            if (e.Button.ToString() == "Buttons4")
                Slot1.Set();
            else if (e.Button.ToString() == "Buttons5")
            {
                Slot1.Recall();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Slot1.Recall();
        }

        private void PresetButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button)?.Name)
            {
                case "_boss1Button":
                    Slot1.SetRecall(560, 1, 0);
                    break;
                case "_boss2Button":
                    Slot1.SetRecall(510, 62, 0);
                    break;
                case "_boss3Button":
                    Slot1.SetRecall(506, 428, 0);
                    break;
                case "_boss4Button":
                    Slot1.SetRecall(144, 71, 0);
                    break;
                case "_part1Button":
                    Slot1.SetRecall(-133, -80, 0);
                    break;
                case "_part2Button":
                    Slot1.SetRecall(-96, -2, 0);
                    break;
                case "_part3Button":
                    Slot1.SetRecall(-144, 0, 0);
                    break;
                case "_part4Button":
                    Slot1.SetRecall(-5, 4, 0);
                    break;
                default:
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
//using SharpDX.DirectInput;

namespace ReadWriteProcessMemory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private KeyboardHook _keyboardHook;
        private GamepadHook _gamepadHook;

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

            //DirectInputController directInputController = new DirectInputController();

            //XInputController xInputController = new XInputController();
            //while (true)
            //{
            //    Thread.Sleep(100);
            //    xInputController.Update();
            //    Console.WriteLine(xInputController.gamepad.Buttons.ToString());
            //}

            //while (true)
            //{
            //    //if (Keyboard.IsKeyDown(Key.A))
            //    //{
            //    //    Console.WriteLine(Key.A.ToString());
            //    //    Thread.Sleep(100);
            //    //}

            //    gamepadHook.Poll();

            //    keyboardHook.Poll();
            //}
        }

        private void test()
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
            //MultiPointer yPosPointer = new MultiPointer(process, "mono.dll", 0x00295BC8, new long[] { 0x20, 0x428, 0x0, 0x20, 0x1B0, 0x18, 0x28, 0x18 });
            try
            {
                //float yPos = yPosPointer.ReadValue();
                //MessageBox.Show(yPos.ToString());
                //xPosPointer.WriteValue(700f);
                yPosPointer.WriteValue(100f);
                //zPosPointer.WriteValue(100f);
                //yPos = yPosPointer.ReadValue();
                //MessageBox.Show(yPos.ToString());
                process.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't read address\n\n" + e.ToString());
            }
        }

        private async void StartPolling()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    _gamepadHook.Poll();
                    _keyboardHook.Poll();
                }
            });
        }

        public float YPos { get; set; }

        private void KeyBoardKeyPressed(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode.ToString());
            if (e.KeyCode == System.Windows.Forms.Keys.NumPad1)
                test();
        }

        private void ControllerButtonPressed(object sender, GamepadButton e)
        {
            Console.WriteLine(e.Button.ToString());
            if (e.Button.ToString() == "POV_0_Left")
                test();
        }

        private void Window_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(e.Key);
                TextBlock textBlock = sender as TextBlock;
                RemoveFocus(textBlock);
                textBlock.Text = key.ToString();
                _keyboardHook.RegisterHotKey(key);
            }
            catch
            {
                MessageBox.Show("Key cannot be bound");
            }
        }

        private async void RemoveFocus(TextBlock textBlock)
        {
            textBlock.Focusable = false;
            await Task.Run(() => 
            {
                Thread.Sleep(50);
            });
            textBlock.Focusable = true;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            textBlock.Focus();
        }
    }

    //class DirectInputController
    //{
    //    public List<Joystick> Joysticks { get; set; }

    //    public DirectInputController()
    //    {
    //        var input = new DirectInput();
    //        var Joysticks = input.GetDevices()
    //            .Where(x => x.Type != DeviceType.Keyboard && x.Type != DeviceType.Mouse)
    //            .Select(x => new Joystick(input, x.InstanceGuid)).ToList();
    //        for (int i = 0; i < Joysticks.Count; i++)
    //        {
    //            var joystick = Joysticks[i];
    //            try
    //            {
    //                joystick.Properties.BufferSize = 128;
    //                joystick.Acquire();
    //            }
    //            catch (Exception ex)
    //            {
    //                Joysticks.RemoveAt(i);
    //                i--;
    //            }
    //        }
    //        var states = Joysticks[0].GetBufferedData();
    //    }

    //    protected bool IsPressed(Joystick joystick, JoystickOffset button, int value)
    //    {
    //        var shortMaskMax = 0xFF00;
    //        var shortMaskMin = 0xFF;

    //        //Console.WriteLine(button.ToString() + " " + value);

    //        if (joystick.Information.Type == DeviceType.Mouse)
    //        {
    //            if (button == JoystickOffset.X
    //                || button == JoystickOffset.Y
    //                || button == JoystickOffset.Buttons0
    //                || button == JoystickOffset.Buttons1)
    //                return false;

    //            if (button == JoystickOffset.Z)
    //                return true;
    //        }

    //        if (button == JoystickOffset.Z
    //            || button == JoystickOffset.X
    //            || button == JoystickOffset.Y
    //            || button == JoystickOffset.RotationX
    //            || button == JoystickOffset.RotationY
    //            || button == JoystickOffset.RotationZ
    //            || button == JoystickOffset.Sliders0
    //            || button == JoystickOffset.Sliders1)
    //            return value >= shortMaskMax || value <= shortMaskMin;

    //        if (button == JoystickOffset.PointOfViewControllers0
    //            || button == JoystickOffset.PointOfViewControllers1
    //            || button == JoystickOffset.PointOfViewControllers2
    //            || button == JoystickOffset.PointOfViewControllers3)
    //            return value != -1;

    //        return value != 0;
    //    }

    //    public void Poll()
    //    {
    //        try
    //        {
    //            Joystick brokenJoystick = null;

    //            var i = 0;

    //            foreach (var joystick in Joysticks)
    //            {
    //                try
    //                {
    //                    joystick.Poll();
    //                    var states = joystick.GetBufferedData();
    //                    foreach (var state in states)
    //                    {
    //                        if (IsPressed(joystick, state.Offset, state.Value) && ButtonPressed != null)
    //                        {
    //                            var buttonName = ToString(joystick, state.Offset, state.Value);
    //                            ButtonPressed(this, new GamepadButton(joystick.Information.InstanceName + " " + joystick.Information.InstanceGuid, buttonName));
    //                        }
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    brokenJoystick = joystick;
    //                    break;
    //                }

    //                ++i;
    //            }

    //            if (brokenJoystick != null)
    //            {
    //                Joysticks.RemoveAt(i);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }

    //    protected string ToString(Joystick joystick, JoystickOffset button, int value)
    //    {
    //        var shortMaskMax = 0xFF00;
    //        var shortMaskMin = 0xFF;

    //        var originalName = button.ToString();

    //        if (button == JoystickOffset.Z
    //           || button == JoystickOffset.X
    //           || button == JoystickOffset.Y
    //           || button == JoystickOffset.RotationX
    //           || button == JoystickOffset.RotationY
    //           || button == JoystickOffset.RotationZ
    //           || button == JoystickOffset.Sliders0
    //           || button == JoystickOffset.Sliders1)
    //            return originalName + (value >= shortMaskMax ? '+' : '-');

    //        if (button == JoystickOffset.PointOfViewControllers0
    //            || button == JoystickOffset.PointOfViewControllers1
    //            || button == JoystickOffset.PointOfViewControllers2
    //            || button == JoystickOffset.PointOfViewControllers3)
    //        {
    //            if (button == JoystickOffset.PointOfViewControllers0)
    //                originalName = "POV_0_";
    //            else if (button == JoystickOffset.PointOfViewControllers1)
    //                originalName = "POV_1_";
    //            else if (button == JoystickOffset.PointOfViewControllers2)
    //                originalName = "POV_2_";
    //            else
    //                originalName = "POV_3_";

    //            if (value < 2250)
    //                return originalName + "Up";
    //            else if (value < 6750)
    //                return originalName + "UpRight";
    //            else if (value < 11250)
    //                return originalName + "Right";
    //            else if (value < 15750)
    //                return originalName + "DownRight";
    //            else if (value < 20250)
    //                return originalName + "Down";
    //            else if (value < 24750)
    //                return originalName + "DownLeft";
    //            else if (value < 29250)
    //                return originalName + "Left";
    //            else if (value < 33750)
    //                return originalName + "UpLeft";
    //            else
    //                return originalName + "Up";
    //        }

    //        return originalName;
    //    }
    //}

    //class XInputController
    //{
    //    Controller controller;
    //    public Gamepad gamepad;
    //    public bool connected = false;
    //    public int deadband = 5000;
    //    public Point leftThumb, rightThumb = new Point(0, 0);
    //    public float leftTrigger, rightTrigger;

    //    public XInputController()
    //    {
    //        controller = new Controller(UserIndex.One);
    //        connected = controller.IsConnected;
    //    }

    //    // Call this method to update all class values
    //    public void Update()
    //    {
    //        if (!connected)
    //            return;

    //        gamepad = controller.GetState().Gamepad;

    //        leftThumb.X = (Math.Abs((float)gamepad.LeftThumbX) < deadband) ? 0 : (float)gamepad.LeftThumbX / short.MinValue * -100;
    //        leftThumb.Y = (Math.Abs((float)gamepad.LeftThumbY) < deadband) ? 0 : (float)gamepad.LeftThumbY / short.MaxValue * 100;
    //        rightThumb.Y = (Math.Abs((float)gamepad.RightThumbX) < deadband) ? 0 : (float)gamepad.RightThumbX / short.MaxValue * 100;
    //        rightThumb.X = (Math.Abs((float)gamepad.RightThumbY) < deadband) ? 0 : (float)gamepad.RightThumbY / short.MaxValue * 100;

    //        leftTrigger = gamepad.LeftTrigger;
    //        rightTrigger = gamepad.RightTrigger;
    //    }
    //}
}

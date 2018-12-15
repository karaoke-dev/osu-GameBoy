using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Emux.GameBoy.Cartridge;
using Emux.GameBoy.Cheating;
using Emux.GameBoy.Graphics;
using Emux.GameBoy.Input;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyContainer : Container
    {
        public event EventHandler<DeviceEventArgs> DeviceLoaded;
        public event EventHandler<DeviceEventArgs> DeviceUnloaded;
        public event EventHandler DeviceChanged;

        #region Component

        private Emux.GameBoy.GameBoy _currentDevice;

        private StreamedExternalMemory _currentExternalMemory;

        private readonly GameBoyScreen _displayScreen;

        private readonly GameBoyPowerLed _powerLed;

        private readonly GameBoyDpad gameBoyDpad;

        private readonly GameBoyButton aGameBoyButton;

        private readonly GameBoyButton bGameBoyButton;

        private readonly GameBoyButton selectGameBoyButton;

        private readonly GameBoyButton startGameBoyButton;

        #endregion

        #region UI

        protected virtual Color4 BackgroundColor => new Color4(181, 181, 178, 255);

        protected virtual Color4 BackgroundLineColor => new Color4(158, 159, 155, 255);

        protected virtual Color4 DisplayBackgroundColor => new Color4(82, 82, 94, 255);

        protected virtual Color4 DisplayScreenColor => DisplayScreenColor1; //Screen off color

        protected virtual Color4 DisplayScreenColor0 => new Color4(224, 248, 208, 255);

        protected virtual Color4 DisplayScreenColor1 => new Color4(136, 192, 112, 255);

        protected virtual Color4 DisplayScreenColor2 => new Color4(52, 104, 86, 255);

        protected virtual Color4 DisplayScreenColor3 => new Color4(8, 24, 32, 255);

        protected virtual Color4 DisplayTextColor => Color4.White;

        protected virtual Color4 Banner1Color => new Color4(110, 19, 79, 255);

        protected virtual Color4 Banner2Color => new Color4(5, 2, 80, 255);

        protected virtual Color4 LedOffColor => new Color4(38, 17, 22, 255);

        protected virtual Color4 LedOnColor => new Color4(204, 68, 79, 255);

        protected virtual Color4 TextColor => new Color4(13, 24, 124, 255);

        protected virtual Color4 DPadButtonBackgroundColor => new Color4(10, 12, 24, 255);

        protected virtual Color4 DPadButtonColor => new Color4(10, 12, 24, 255);

        protected virtual Color4 DPadButtonPressedColor => new Color4(34, 41, 83, 255);

        protected virtual Color4 ABButtonColor => new Color4(154, 31, 85, 255);

        protected virtual Color4 ABButtonPressedColor => new Color4(109, 22, 62, 255);

        protected virtual Color4 OptionButtonColor => new Color4(112, 111, 119, 255);

        protected virtual Color4 OptionButtonPressedColor => new Color4(78, 77, 81, 255);

        protected virtual bool ForceOriginalGameBoy => false;

        #endregion

        #region Keys

        public virtual Key UpKey { get => gameBoyDpad.UpButtonKey; set => gameBoyDpad.UpButtonKey = value; }

        public virtual Key DownKey { get => gameBoyDpad.DownButtonKey; set => gameBoyDpad.DownButtonKey = value; }

        public virtual Key LeftKey { get => gameBoyDpad.LeftButtonKey; set => gameBoyDpad.LeftButtonKey = value; }

        public virtual Key RightKey { get => gameBoyDpad.RightButtonKey; set => gameBoyDpad.RightButtonKey = value; }

        public virtual Key AKey { get => aGameBoyButton.ButtonKey; set => aGameBoyButton.ButtonKey = value; }

        public virtual Key BKey { get => bGameBoyButton.ButtonKey; set => bGameBoyButton.ButtonKey = value; }

        public virtual Key SelectKey { get => selectGameBoyButton.ButtonKey; set => selectGameBoyButton.ButtonKey = value; }

        public virtual Key StartKey { get => startGameBoyButton.ButtonKey; set => startGameBoyButton.ButtonKey = value; }

        #endregion

        public GameBoyContainer()
        {
            //Colors


            //Keys
            UpKey = Key.Up;
            DownKey = Key.Down;
            LeftKey = Key.Left;
            RightKey = Key.Right;
            AKey = Key.Z;
            BKey = Key.X;
            SelectKey = Key.ShiftLeft;
            StartKey = Key.Enter;

            Name = "Gameboy";
            //Initial Ui
            AddInternal(new Container
            {
                Name = "Gameboy body",
                Width = 275,
                Height = 450,
                Masking = true,
                CornerRadius = 10,
                Children = new Drawable[]
                {
                    new Container
                    {
                        Name = "Background area",
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Name = "Background",
                                Colour = BackgroundColor,
                                RelativeSizeAxes = Axes.Both
                            },
                            new Box
                            {
                                Name = "Center line",
                                Colour = BackgroundLineColor,
                                RelativeSizeAxes = Axes.X,
                                Height = 3,
                                Y = 20
                            },
                            new Box
                            {
                                Name = "Left line",
                                Colour = BackgroundLineColor,
                                Width = 3,
                                Height = 20,
                                X = 20,
                            },
                            new Box
                            {
                                Name = "Right line",
                                Colour = BackgroundLineColor,
                                Width = 3,
                                Height = 20,
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.TopRight,
                                X = -20,
                            },
                            new FillFlowContainer
                            {
                                Name = "Logo",
                                Anchor = Anchor.TopLeft,
                                Origin = Anchor.TopLeft,
                                X = 16,
                                Y = 210,
                                Direction = FillDirection.Horizontal,
                                Children = new Drawable[]
                                {
                                    new SpriteText
                                    {
                                        Colour = TextColor,
                                        TextSize = 15,
                                        Text = "Nintendo",
                                        Scale = new Vector2(1.3f,1)
                                    },
                                    new SpriteText
                                    {
                                        Colour = TextColor,
                                        TextSize = 25,
                                        Text = "GAME BOY",
                                        X = 40,
                                        Y = 5,
                                    },
                                    new SpriteText
                                    {
                                        Colour = TextColor,
                                        TextSize = 10,
                                        Text = "TM",
                                        X = 120,
                                        Y = 5,
                                    },
                                }
                            },
                            new GameBoySpeaker(BackgroundLineColor,5)
                            {
                                Name = "Speaker",
                                Width = 100,
                                Height = 40,
                                Spacing = new Vector2(8),
                                Anchor = Anchor.BottomRight,
                                Origin = Anchor.BottomRight,
                                X = 20,
                                Y = -60,
                                Rotation = -30
                            },
                            new Box
                            {
                                Name = "Phones",
                                Colour = BackgroundLineColor,
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Width = 30,
                                Height = 5,
                                Y = -5,
                            }
                        }
                    },
                    new Container
                    {
                        Name = "Screen area",
                        Width = 240,
                        Height = 175,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Masking = true,
                        Y = 35,
                        CornerRadius = 5,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Name = "Background",
                                Colour = DisplayBackgroundColor,
                                RelativeSizeAxes = Axes.Both,
                            },
                            new Container
                            {
                                Name = "Banner",
                                RelativeSizeAxes = Axes.X,
                                Y = 6,
                                Padding = new MarginPadding{Left = 10,Right = 10},
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 2f,
                                        Colour = Banner1Color,
                                        Y = 1
                                    },
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 2f,
                                        Colour = Banner2Color,
                                        Y = 6
                                    },
                                    new Container
                                    {
                                        Name = "Text",
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        X = 20,
                                        Width = 113,
                                        Height = 10,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                Colour = DisplayBackgroundColor,
                                                RelativeSizeAxes = Axes.Both,
                                                X = -3
                                            },
                                            new SpriteText
                                            {
                                                TextSize = 9,
                                                Text = "DOT MATRIX WITH STEREO SOUND",
                                                Colour = DisplayTextColor,
                                            }
                                        }
                                    }
                                }
                            },
                             _powerLed = new GameBoyPowerLed
                            {
                                Name = "Power led",
                                Width = 8,
                                Height = 8,
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                PowerOffColor = LedOffColor,
                                PowerOnColor = LedOnColor,
                                X = 15,
                                Y = -12
                            },
                            new SpriteText
                            {
                                Name = "Battery text",
                                Text = "BATTERY",
                                TextSize = 9,
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Colour = DisplayTextColor,
                                X = 10,
                            },
                            new Container
                            {
                                Name = "Screen",
                                Anchor = Anchor.Centre,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        Name = "Screen Background",
                                        Colour = Color4.Black,
                                        Origin = Anchor.Centre,
                                        Width = 160 * 0.91f,
                                        Height = 144 * 0.91f,
                                    },
                                    _displayScreen = new GameBoyScreen
                                    {
                                        Name = "Screen Sprite",
                                        Origin = Anchor.Centre,
                                        Width = 160 * 0.9f,
                                        Height = 144 * 0.9f,
                                        ScreenColor = DisplayScreenColor,
                                    }
                                }
                            },
                        }
                    },
                    new Container
                    {
                        Name = "Button area",
                        Width = 230,
                        Height = 100,
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Y = 280,
                        Children = new Drawable[]
                        {
                            gameBoyDpad = new GameBoyDpad(DPadButtonBackgroundColor)
                            {
                                Name = "Gamepad",
                                ButtonColor = DPadButtonColor,
                                ButtonPressedColor = DPadButtonPressedColor,
                                KeyPressedEvent = (direction,press ) =>{
                                    switch(direction)
                                    {
                                        case GameBoyDpad.DPadDirection.Up :
                                            ButtonPressChanged(GameBoyPadButton.Up,press);
                                        break;
                                        case GameBoyDpad.DPadDirection.Down :
                                            ButtonPressChanged(GameBoyPadButton.Down,press);
                                        break;
                                        case GameBoyDpad.DPadDirection.Left :
                                            ButtonPressChanged(GameBoyPadButton.Left,press);
                                        break;
                                        case GameBoyDpad.DPadDirection.Right :
                                            ButtonPressChanged(GameBoyPadButton.Right,press);
                                        break;
                                    }
                                },
                                Width = 70,
                                Height = 70,
                                Y = -10,
                            },
                            aGameBoyButton = new GameBoyButton
                            {
                                Name = "A Button",
                                ButtonText = "A",
                                TextColor = TextColor,
                                ButtonColor = ABButtonColor,
                                ButtonPressedColor = ABButtonPressedColor,
                                KeyPressedEvent = (press)=> ButtonPressChanged(GameBoyPadButton.A,press),
                                Width = 35,
                                Height = 35,
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.CentreRight,
                                Rotation = -30
                            },
                            bGameBoyButton = new GameBoyButton
                            {
                                Name = "B Button",
                                ButtonText = "B",
                                TextColor = TextColor,
                                ButtonColor = ABButtonColor,
                                ButtonPressedColor = ABButtonPressedColor,
                                KeyPressedEvent = (press)=> ButtonPressChanged(GameBoyPadButton.B,press),
                                Width = 35,
                                Height = 35,
                                Anchor = Anchor.TopRight,
                                Origin = Anchor.CentreRight,
                                X = -45,
                                Y = 25,
                                Rotation = -30
                            },
                            selectGameBoyButton = new GameBoyButton
                            {
                                Name = "Select Button",
                                ButtonText = "SELECT",
                                TextColor = TextColor,
                                ButtonColor = OptionButtonColor,
                                ButtonPressedColor = OptionButtonPressedColor,
                                KeyPressedEvent = (press)=> ButtonPressChanged(GameBoyPadButton.Select,press),
                                Width = 33,
                                Height = 10,
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Rotation = -30,
                                X = -20,
                            },
                            startGameBoyButton = new GameBoyButton
                            {
                                Name = "Start Button",
                                ButtonText = "START",
                                TextColor = TextColor,
                                ButtonColor = OptionButtonColor,
                                ButtonPressedColor = OptionButtonPressedColor,
                                KeyPressedEvent = (press)=> ButtonPressChanged(GameBoyPadButton.Start,press),
                                Width = 33,
                                Height = 10,
                                Anchor = Anchor.BottomCentre,
                                Origin = Anchor.BottomCentre,
                                Rotation = -30,
                                X = 20,
                            }
                        }
                    }
                }
            });

            _displayScreen.ScreenOff();

            AudioMixer = new GameBoyMixer();
            //var player = new DirectSoundOut();
            //player.Init(AudioMixer);
            //player.Play();

            GamesharkController = new GamesharkController();
            Breakpoints = new Dictionary<ushort, BreakpointInfo>();
        }

        protected sealed override void AddInternal(Drawable drawable)
        {
            base.AddInternal(drawable);
        }

        public GameBoyMixer AudioMixer
        {
            get;
        }

        public Emux.GameBoy.GameBoy CurrentDevice
        {
            get { return _currentDevice; }
            private set
            {
                if (_currentDevice != value)
                {
                    _currentDevice = value;
                    if (value != null)
                    {
                        AudioMixer.Connect(value.Spu);
                        GamesharkController.Device = value;

                        //Connect output
                        _currentDevice.Gpu.VideoOutput = _displayScreen;
                    }
                    OnDeviceChanged();
                }
            }
        }

        public GamesharkController GamesharkController
        {
            get;
        }

        public IDictionary<ushort, BreakpointInfo> Breakpoints
        {
            get;
        }

        public void UnloadDevice()
        {
            var device = _currentDevice;
            if (device != null)
            {
                device.Terminate();
                _currentExternalMemory.Dispose();
                _currentDevice = null;
                _displayScreen.ScreenOff();
                OnDeviceUnloaded(new DeviceEventArgs(device));
            }
        }

        public void LoadRom(string romFilePath, string ramFilePath)
        {
            if(string.IsNullOrEmpty(romFilePath) || !File.Exists(romFilePath))
            { 
                //https://github.com/pashutk/flappybird-gameboy
                var defaultRomName = "defaultRom_20181206";

                //get rom from resource
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"osu.Framework.Resources.rom.{defaultRomName}.gb";

                //change sav name
                ramFilePath = defaultRomName + ".sav";

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                { 
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    //Load default rom
                    LoadRom(memoryStream.ToArray(), File.Open(ramFilePath, FileMode.OpenOrCreate));
                }
            }
            else
            { 
                //Load default rom
                LoadRom(File.ReadAllBytes(romFilePath), File.Open(ramFilePath, FileMode.OpenOrCreate));
            }
        }

        public void LoadRom(byte[] romBytes, Stream ramStream)
        {
            UnloadDevice();
            _currentExternalMemory = new StreamedExternalMemory(ramStream);
            var cartridge = new EmulatedCartridge(romBytes, _currentExternalMemory);
            _currentExternalMemory.SetBufferSize(cartridge.ExternalRamSize);
            CurrentDevice = new Emux.GameBoy.GameBoy(cartridge, RecreateTicker(), !ForceOriginalGameBoy);
            ApplyColorPalettes();
            OnDeviceLoaded(new DeviceEventArgs(CurrentDevice));
        }

        public void RunDevice()
        {
            if (CurrentDevice != null && !CurrentDevice.Cpu.Running)
            {
                CurrentDevice.Cpu.Run();
            }
        }

        public void ResetDevice()
        {
            CurrentDevice.Reset();
        }

        public void StepDevice()
        {
            CurrentDevice.Cpu.Step();
        }

        public void BreakDevice()
        {
            CurrentDevice.Cpu.Break();
        }

        public void SetBreakPoint()
        {
            CurrentDevice.Cpu.Step();
        }

        public void ResetBreakPoint()
        {
            CurrentDevice.Cpu.ClearBreakpoints();
        }

        GameBoyTicker RecreateTicker()
        {
            RemoveAll(x => x is GameBoyTicker);
            var ticker = new GameBoyTicker();
            Add(ticker);
            return ticker;
        }

        protected override void Dispose(bool isDisposing)
        {
            //unload device
            UnloadDevice();
            base.Dispose(isDisposing);
        }

        protected virtual void OnDeviceChanged()
        {
            _powerLed.LedOff();
            DeviceChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDeviceLoaded(DeviceEventArgs e)
        {
            _powerLed.LedOn();
            DeviceLoaded?.Invoke(this, e);
        }

        protected virtual void OnDeviceUnloaded(DeviceEventArgs e)
        {
            DeviceUnloaded?.Invoke(this, e);
        }

        private Color ConvertColor(Color4 color)
        {
            return new Color
            {
                R = (byte)(color.R * 255),
                G = (byte)(color.G * 255),
                B = (byte)(color.B * 255)
            };
        }

        private void ApplyColorPalettes()
        {
            if (CurrentDevice != null)
            {
                CurrentDevice.Gpu.Color0 = ConvertColor(DisplayScreenColor0);
                CurrentDevice.Gpu.Color1 = ConvertColor(DisplayScreenColor1);
                CurrentDevice.Gpu.Color2 = ConvertColor(DisplayScreenColor2);
                CurrentDevice.Gpu.Color3 = ConvertColor(DisplayScreenColor3);
            }
        }

        private void ButtonPressChanged(GameBoyPadButton button, bool press)
        {
            if (press)
            {
                CurrentDevice.KeyPad.PressedButtons |= button;
            }
            else
            {
                CurrentDevice.KeyPad.PressedButtons &= ~button;
            }
        }
    }
}

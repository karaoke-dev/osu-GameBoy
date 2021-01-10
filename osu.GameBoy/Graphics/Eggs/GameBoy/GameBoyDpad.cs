using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyDpad : Container
    {
        private readonly Triangle _upButton;

        private readonly Triangle _downButton;

        private readonly Triangle _leftButton;

        private readonly Triangle _rightButton;

        public Color4 ButtonColor { get; set; }

        public Color4 ButtonPressedColor { get; set; }

        public Key UpButtonKey { get; set; }

        public Key DownButtonKey { get; set; }

        public Key LeftButtonKey { get; set; }

        public Key RightButtonKey { get; set; }

        public Action<DPadDirection, bool> KeyPressedEvent;

        public GameBoyDpad(Color4 backgroundColor)
        {
            Children = new Drawable[]
            {
                new Container
                {
                    Name = "Background",
                    Masking = true,
                    CornerRadius = 3,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    FillAspectRatio = 0.37f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColor
                    }
                },
                new Container
                {
                    Name = "Background",
                    Masking = true,
                    CornerRadius = 3,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    FillAspectRatio = 2.7f,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = backgroundColor
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        _upButton = new Triangle
                        {
                            Name = "Up",
                            Width = 10,
                            Height = 10,
                            Colour = Color4.Black,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.Centre
                        },
                        _downButton = new Triangle
                        {
                            Name = "Down",
                            Width = 10,
                            Height = 10,
                            Colour = Color4.Black,
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.Centre,
                            Rotation = 180
                        },
                        _leftButton = new Triangle
                        {
                            Name = "Left",
                            Width = 10,
                            Height = 10,
                            Colour = Color4.Black,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.Centre,
                            Rotation = 270
                        },
                        _rightButton = new Triangle
                        {
                            Name = "Right",
                            Width = 10,
                            Height = 10,
                            Colour = Color4.Black,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.Centre,
                            Rotation = 90
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            _upButton.Colour = ButtonColor;
            _downButton.Colour = ButtonColor;
            _leftButton.Colour = ButtonColor;
            _rightButton.Colour = ButtonColor;
            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            ChangeKeyEvent(e);
            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            ChangeKeyEvent(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            ChangeKeyEvent(e);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            ChangeKeyEvent(e);
        }

        protected virtual void ChangeKeyEvent(UIEvent uiEvent)
        {
            DPadDirection? direction = null;
            var press = false;

            switch (uiEvent)
            {
                case KeyDownEvent keyDownEvent:
                    press = true;
                    direction = GetDPadDirection(keyDownEvent.Key);
                    break;
                case KeyUpEvent keyUpEvent:
                    direction = GetDPadDirection(keyUpEvent.Key);
                    break;
                case MouseDownEvent mouseDownEvent:
                    press = true;
                    direction = GetDPadDirection(mouseDownEvent.MouseDownPosition);
                    break;
                case MouseUpEvent mouseUpEvent:
                    direction = GetDPadDirection(mouseUpEvent.MouseDownPosition);
                    break;
            }

            if (direction == null) 
                return;
            
            ChangeKeypadColor(direction.Value, press);
            KeyPressedEvent?.Invoke(direction.Value, press);
        }

        protected virtual DPadDirection? GetDPadDirection(Key key)
        {
            if (key == UpButtonKey)
                return DPadDirection.Up;
            if (key == DownButtonKey)
                return DPadDirection.Down;
            if (key == LeftButtonKey)
                return DPadDirection.Left;
            if (key == RightButtonKey)
                return DPadDirection.Right;
            
            return null;
        }

        protected virtual DPadDirection? GetDPadDirection(Vector2 mouseLocalPosition)
        {
            //TODO : implement
            return null;
        }

        protected virtual void ChangeKeypadColor(DPadDirection direction, bool press)
        {
            var buttonColor = press ? ButtonPressedColor : ButtonColor;
            switch (direction)
            {
                case DPadDirection.Up:
                    _upButton.Colour = buttonColor;
                    break;
                case DPadDirection.Down:
                    _downButton.Colour = buttonColor;
                    break;
                case DPadDirection.Left:
                    _leftButton.Colour = buttonColor;
                    break;
                case DPadDirection.Right:
                    _rightButton.Colour = buttonColor;
                    break;
            }
        }

        public enum DPadDirection
        {
            Up,

            Down,

            Left,

            Right,
        }
    }
}
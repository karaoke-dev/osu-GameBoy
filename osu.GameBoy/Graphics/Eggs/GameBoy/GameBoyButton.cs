using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyButton : Container, IHasTooltip
    {
        private readonly Circle _circle;

        private readonly SpriteText _spriteText;

        public string ButtonText
        {
            get => _spriteText.Text;
            set => _spriteText.Text = value;
        }

        public Key ButtonKey { get; set; }

        public Color4 ButtonColor { get; set; }

        public Color4 ButtonPressedColor { get; set; }

        public Color4 TextColor
        {
            get => _spriteText.Colour;
            set => _spriteText.Colour = value;
        }

        public string TooltipText => ButtonKey.ToString();

        public Action<bool> KeyPressedEvent;

        public GameBoyButton()
        {
            Children = new Drawable[]
            {
                _circle = new Circle()
                {
                    RelativeSizeAxes = Axes.Both
                },
                _spriteText = new SpriteText
                {
                    Font = new FontUsage(size: 15),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.TopCentre,
                    Y = 5
                }
            };
        }

        protected override void LoadComplete()
        {
            _circle.Colour = ButtonColor;
            base.LoadComplete();
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == ButtonKey)
            {
                _circle.Colour = ButtonPressedColor;
                KeyPressedEvent?.Invoke(true);
            }
            return base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (e.Key == ButtonKey)
            {
                _circle.Colour = ButtonColor;
                KeyPressedEvent?.Invoke(false);
            }
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            _circle.Colour = ButtonPressedColor;
            KeyPressedEvent?.Invoke(true);
            return base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            _circle.Colour = ButtonColor;
            KeyPressedEvent?.Invoke(false);
        }
    }
}

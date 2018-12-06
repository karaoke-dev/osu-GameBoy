using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyPowerLed : Circle
    {
        public Color4 PowerOffColor { get; set; }

        public Color4 PowerOnColor { get; set; }

        public virtual void LedOff()
        {
            Colour = PowerOffColor;
        }

        public virtual void LedOn()
        {
            Colour = PowerOnColor;
        }
    }
}

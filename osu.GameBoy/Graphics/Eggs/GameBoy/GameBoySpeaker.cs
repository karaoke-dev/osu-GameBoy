using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public sealed class GameBoySpeaker : FillFlowContainer
    {
        public GameBoySpeaker(Color4 speakerColor, float speakerWidth)
        {
            for (int i = 0; i < 6; i++)
            {
                Add(new Box
                {
                    Colour = speakerColor,
                    Width = speakerWidth,
                    RelativeSizeAxes = Axes.Y,
                });
            }
        }
    }
}

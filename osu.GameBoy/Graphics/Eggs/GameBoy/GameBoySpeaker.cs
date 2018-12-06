using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public sealed class GameBoySpeaker : FillFlowContainer
    {
        public GameBoySpeaker(float speakerWidth)
        {
            for (int i = 0; i < 6; i++)
            {
                Add(new Box
                {
                    Width = speakerWidth,
                    RelativeSizeAxes = Axes.Y,
                });
            }
        }

        public Color4 SperkeColour
        {
            set
            { 
                Children.ForEach(x => x.Colour = value);
            }
        }
    }
}

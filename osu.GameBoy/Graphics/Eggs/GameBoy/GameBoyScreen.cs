using Emux.GameBoy.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;
using SixLabors.ImageSharp.PixelFormats;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyScreen : Sprite, IVideoOutput
    {
        public Color4 ScreenColor { get; set; }

        public GameBoyScreen()
        {
            Texture = new Texture(160, 144);
        }

        public void ScreenOff()
        {
            var rByte = (byte)(ScreenColor.R * 255);
            var gByte = (byte)(ScreenColor.G * 255);
            var bByte = (byte)(ScreenColor.B * 255);
            var rawData = new byte[160 * 144 * sizeof(int)];

            for (int i = 0; i < rawData.Length; i += 4)
            {
                rawData[i] = rByte;
                rawData[i + 1] = gByte;
                rawData[i + 2] = bByte;
                rawData[i + 3] = 255;
            }
            var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(rawData, 160, 144);
            Texture.SetData(new TextureUpload(image));
        }

        public void RenderFrame(byte[] pixelData)
        {
            var rawData = new byte[160 * 144 * sizeof(int)];

            for (int i = 0, j = 0; j < pixelData.Length; i += 4, j += 3)
            {
                rawData[i] = pixelData[j];
                rawData[i + 1] = pixelData[j + 1];
                rawData[i + 2] = pixelData[j + 2];
                rawData[i + 3] = 255;
            }
            var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(rawData, 160, 144);
            Texture.SetData(new TextureUpload(image));
        }
    }
}

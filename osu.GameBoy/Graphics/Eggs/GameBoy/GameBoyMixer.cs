using System.Collections.Generic;
using Emux.GameBoy.Audio;
using osu.Framework.Graphics.Eggs.Gameboy;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyMixer
    {
        protected int _mixer;

        public GameBoyMixer()
        {
            Channels = new List<AudioChannelOutput>
            {
                new AudioChannelOutput(this, "Square + Sweep"),
                new AudioChannelOutput(this, "Square"),
                new AudioChannelOutput(this, "Wave"),
                new AudioChannelOutput(this, "Noise"),
            }.AsReadOnly();

            //_mixer = BassMix.CreateMixerStream(44100, 2, BassFlags.Default);

            foreach (var channel in Channels)
            {
                //BassMix.MixerAddChannel(_mixer, _recording, BassFlags.MixerDownMix);
            }
        }

        public IList<AudioChannelOutput> Channels
        {
            get;
        }

        public void Connect(GameBoySpu spu)
        {
            for (var i = 0; i < spu.Channels.Count; i++)
            {
                var channel = spu.Channels[i];
                channel.ChannelOutput = Channels[i];
                channel.ChannelVolume = 0.05f;
            }
        }
    }
}

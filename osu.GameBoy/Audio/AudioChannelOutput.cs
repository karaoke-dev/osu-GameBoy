using System;
using System.Collections.Generic;
using System.Text;
using Emux.GameBoy.Audio;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics.Eggs.GameBoy;

namespace osu.Framework.Graphics.Eggs.Gameboy
{
    public class AudioChannelOutput : IAudioChannelOutput
    {
        public AudioChannelOutput(GameBoyMixer mixer, string name)
        {
            if (mixer == null)
                throw new ArgumentNullException(nameof(mixer));
        }

        public bool Enabled { get; set; } = true;

        public int PlaybackConcurrency { get; set; }

        public int SampleId { get; set; }

        public int SampleRate { get; } = 44100;

        private SampleChannel sample;

        public void BufferSoundSamples(float[] sampleData, int offset, int length)
        {
            //osu-framework的方法，不太適用
            var newSampleData = new byte[length * sizeof(float)];
            if (Enabled)
                Buffer.BlockCopy(sampleData, offset * sizeof(float), newSampleData, 0, length * sizeof(float));

            sample = new Framework.Audio.SampleChannelBass(new SampleBass(newSampleData), a => { });
            sample.Play();

            //用原本NAudio格式下去改
            /*
            byte[] newSampleData = new byte[length * sizeof(float)];
            if (Enabled)
                Buffer.BlockCopy(sampleData, offset * sizeof(float), newSampleData, 0, length * sizeof(float));

            SampleId = Bass.SampleLoad(newSampleData, offset, length, PlaybackConcurrency, BassFlags.Default | BassFlags.SampleOverrideLongestPlaying);
            var channel = Bass.SampleGetChannel(SampleId);
            if(channel!=0)
                Bass.ChannelPlay(channel, false);
           */
        }
    }
}

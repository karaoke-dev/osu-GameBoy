using System;
using System.Collections.Generic;
using System.Text;
using Emux.GameBoy.Audio;
using ManagedBass;
using ManagedBass.Fx;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Eggs.GameBoy;

namespace osu.Framework.Graphics.Eggs.Gameboy
{
    public class AudioChannelOutput : Track ,IAudioChannelOutput
    {
        private double currentTime;

        private volatile bool isRunning;

        /// <summary>
        /// The handle for this track, if there is one.
        /// </summary>
        private int activeStream;

        /// <summary>
        /// The handle for adjusting tempo.
        /// </summary>
        private int tempoAdjustStream;

        public AudioChannelOutput(GameBoyMixer mixer, string name)
        {
            if (mixer == null)
                throw new ArgumentNullException(nameof(mixer));

            activeStream = Bass.CreateStream(SampleRate, 2, BassFlags.Decode | BassFlags.Prescan, StreamProcedureType.Push);

            Bass.ChannelPlay(activeStream);

            /*
            // We assign the BassFlags.Decode streams to the device "bass_nodevice" to prevent them from getting
            // cleaned up during a Bass.Free call. This is necessary for seamless switching between audio devices.
            // Further, we provide the flag BassFlags.FxFreeSource such that freeing the activeStream also frees
            // all parent decoding streams.
            const int bass_nodevice = 0x20000;

            //I don't know but just copy it
            Bass.ChannelSetDevice(activeStream, bass_nodevice);
            tempoAdjustStream = BassFx.TempoCreate(activeStream, BassFlags.Decode | BassFlags.FxFreeSource);
            Bass.ChannelSetDevice(activeStream, bass_nodevice);
            activeStream = BassFx.ReverseCreate(tempoAdjustStream, 5f, BassFlags.Default | BassFlags.FxFreeSource);


            Bass.ChannelSetAttribute(activeStream, ChannelAttribute.TempoUseQuickAlgorithm, 1);
            Bass.ChannelSetAttribute(activeStream, ChannelAttribute.TempoOverlapMilliseconds, 4);
            Bass.ChannelSetAttribute(activeStream, ChannelAttribute.TempoSequenceMilliseconds, 30);
            */
        }

        public int SampleRate { get; } = 44100;

        public override double CurrentTime => currentTime;

        public override bool IsRunning => isRunning;

        public void BufferSoundSamples(float[] sampleData, int offset, int length)
        {
            Bass.StreamPutData(activeStream, sampleData, length);

            

            //audiobass's method
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

        public override bool Seek(double seek)
        {
            throw new NotImplementedException();
        }
    }
}

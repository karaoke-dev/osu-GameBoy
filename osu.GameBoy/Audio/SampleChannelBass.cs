using System;
using ManagedBass;
using osu.Framework.Audio.Sample;

namespace osu.Framework.Audio
{
    internal class SampleChannelBass : SampleChannel
    {
        private volatile int channel;
        private volatile bool playing;

        public override bool IsLoaded => Sample.IsLoaded;

        private float initialFrequency;

        public SampleChannelBass(Sample.Sample sample, Action<SampleChannel> onPlay)
            : base(sample, onPlay)
        {
        }

        public override void Play(bool restart = false)
        {
            EnqueueAction(() =>
            {
                if (!IsLoaded)
                {
                    channel = 0;
                    return;
                }

                // We are creating a new channel for every playback, since old channels may
                // be overridden when too many other channels are created from the same sample.
                channel = ((SampleBass)Sample).CreateChannel();
                Bass.ChannelGetAttribute(channel, ChannelAttribute.Frequency, out initialFrequency);
            });

            //InvalidateState();

            EnqueueAction(() =>
            {
                if (channel != 0)
                    Bass.ChannelPlay(channel, restart);
            });

            // Needs to happen on the main thread such that
            // Played does not become true for a short moment.
            playing = true;

            base.Play(restart);
        }

        protected override void UpdateState()
        {
            base.UpdateState();
            playing = channel != 0 && Bass.ChannelIsActive(channel) != 0;
        }

        public override void Stop()
        {
            if (channel == 0) return;

            base.Stop();

            EnqueueAction(() =>
            {
                Bass.ChannelStop(channel);
                // ChannelStop frees the channel.
                channel = 0;
            });
        }

        public override bool Playing => playing;
    }
}

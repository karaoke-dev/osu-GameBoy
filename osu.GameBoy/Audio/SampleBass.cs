using System.Collections.Concurrent;
using System.Threading.Tasks;
using ManagedBass;

namespace osu.Framework.Audio
{
    internal class SampleBass : Sample.Sample
    {
        private volatile int sampleId;

        public override bool IsLoaded => sampleId != 0;

        public SampleBass(byte[] data, ConcurrentQueue<Task> customPendingActions = null, int concurrency = DEFAULT_CONCURRENCY)
            : base(concurrency)
        {
            if (customPendingActions != null)
                PendingActions = customPendingActions;

            EnqueueAction(() => { sampleId = Bass.SampleLoad(data, 0, data.Length, PlaybackConcurrency, BassFlags.Default | BassFlags.SampleOverrideLongestPlaying); });
        }

        protected override void Dispose(bool disposing)
        {
            Bass.SampleFree(sampleId);
            base.Dispose(disposing);
        }

        public int CreateChannel() => Bass.SampleGetChannel(sampleId);
    }
}

using System;
using Emux.GameBoy.Cpu;
using osu.Framework.Graphics.Containers;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class GameBoyTicker : Container, IClock
    {
        public event EventHandler Tick;

        private bool _tick;
        private double _lastTickleTime;

        public void Start()
        {
            _tick = true;
        }

        public void Stop()
        {
            _tick = false;
        }

        protected override void Update()
        {
            if (!_tick) 
                return;
            
            var time = Time.Current;

            //Limit to 60HZ par second
            if (time - _lastTickleTime > (1.0f / 60.0f) * 1000)
            {
                Tick?.Invoke(this, EventArgs.Empty);
                _lastTickleTime = time;
            }
        }
    }
}

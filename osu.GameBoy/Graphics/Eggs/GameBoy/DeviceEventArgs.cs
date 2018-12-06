using System;

namespace osu.Framework.Graphics.Eggs.GameBoy
{
    public class DeviceEventArgs : EventArgs
    {
        public DeviceEventArgs(Emux.GameBoy.GameBoy device)
        {
            Device = device;
        }

        public Emux.GameBoy.GameBoy Device
        {
            get;
        }
    }
}

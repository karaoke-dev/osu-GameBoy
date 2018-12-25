using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;

namespace osu.Framework.Eggs.Gameboy.Tests
{
    internal class TestGame : Game
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(Assembly.GetExecutingAssembly().Location), "Resources"));
        }
    }
}

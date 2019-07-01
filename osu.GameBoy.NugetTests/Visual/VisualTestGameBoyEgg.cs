using System.IO;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Eggs.GameBoy;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Eggs.Gameboy.Tests.Visual
{
    /// <summary>
    /// NOTE :
    /// You need to download GB rom first
    /// </summary>
    [TestFixture]
    public sealed class VisualTestGameBoyEgg : TestScene
    {
        private readonly GameBoyContainer gameBoyContainer;

        public VisualTestGameBoyEgg()
        {
            CursorContainer cursor = new CursorContainer();
            Add(cursor);
            
            Add(new TooltipContainer(cursor)
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    gameBoyContainer = new GameBoyContainer
                    {
                        Scale = new Vector2(1.0f)
                    }
                }
            });

            InitialTestStep();
        }

        void InitialTestStep()
        {
            //load 
            AddStep("Load game", () =>
            {
                //NOTE : 
                //You need to download GB rom first
                string romPath = "Resources/ROM/Tetris.gb";
                gameBoyContainer.LoadRom(romPath, Path.ChangeExtension(romPath, ".sav"));
            });

            //load 
            AddStep("Load default", () =>
            {
                string romPath = "";
                gameBoyContainer.LoadRom(romPath, Path.ChangeExtension(romPath, ".sav"));
            });

            //run game
            AddStep("Run", () =>
            {
                gameBoyContainer.RunDevice();
            });

            //reset and reload game
            AddStep("Reset", () =>
            {
                gameBoyContainer.ResetDevice();
            });

            //stop game
            AddStep("Step", () =>
            {
                gameBoyContainer.StepDevice();
            });

            //break game
            AddStep("Break", () =>
            {
                gameBoyContainer.BreakDevice();
            });

            //set breakPoint
            AddStep("Set BreakPoint", () =>
            {
                gameBoyContainer.SetBreakPoint();
            });

            //clean all breakpoint
            AddStep("Reset BreakPoint", () =>
            {
                gameBoyContainer.ResetBreakPoint();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using FreneticGameCore;
using Test3DGame.MainGame;

namespace Test3DGame
{
    /// <summary>
    /// Central entry class for the program.
    /// </summary>
    public class T3DProgram : Program
    {
        /// <summary>
        /// The name of the game.
        /// </summary>
        public const string T3DGameName = "Test3DGame";

        /// <summary>
        /// The version of the game. Automatically read from file.
        /// </summary>
        public static readonly string T3DGameVersion = Assembly.GetCallingAssembly().GetName().Version.ToString();

        /// <summary>
        /// The description of the game version.
        /// </summary>
        public const string T3dVersionDescription = "Pre-Alpha";

        public T3DProgram()
            : base(T3DGameName, T3DGameVersion, T3dVersionDescription)
        {
        }

        /// <summary>
        /// Main entry method.
        /// </summary>
        /// <param name="args">Command line arguments, if any.</param>
        static void Main(string[] args)
        {
            PreInit(new T3DProgram());
            SysConsole.Init();
            Game game = new Game();
            game.Start();
        }
    }
}

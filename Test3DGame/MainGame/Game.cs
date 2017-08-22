using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticGameCore;
using FreneticGameGraphics;
using FreneticGameGraphics.ClientSystem;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using FreneticGameGraphics.LightingSystem;
using OpenTK.Input;
using Test3DGame.GameEntities;

namespace Test3DGame.MainGame
{
    /// <summary>
    /// The game's entry point.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The primary backing game engine.
        /// </summary>
        public GameEngine3D Engine;

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            Engine = new GameEngine3D()
            {
                Forward_Shadows = true
            };
            Engine.OnWindowLoad += Engine_WindowLoad;
            Engine.Start();
        }

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            Engine.Window.KeyDown += Window_KeyDown;
            Engine.SpawnEntity(new FreeCamera());
            Engine.SpawnEntity(new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = Engine.Models.Cube,
                Scale = new Location(1, 10, 10),
                RenderAt = new Location(10, 0, 0),
                DiffuseTexture = Engine.Textures.White
            });
            PointLight pl = new PointLight(new Location(7, 0, 3), 15f, Location.One);
            Engine.MainView.Lights.Add(pl);
        }

        /// <summary>
        /// Handles escape key pressing to exit.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Engine.Window.Close();
            }
        }
    }
}

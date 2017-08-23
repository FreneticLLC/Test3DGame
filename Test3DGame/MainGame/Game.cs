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
using FreneticGameCore.EntitySystem;
using FreneticGameCore.EntitySystem.PhysicsHelpers;

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
            Engine.MainView.ShadowTexSize = () => 1024;
            Engine.OnWindowLoad += Engine_WindowLoad;
            Engine.Start();
        }

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            Engine.Window.KeyDown += Window_KeyDown;
            // Ground
            Engine.SpawnEntity(new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = Engine.Models.Cube,
                Scale = new Location(100, 100, 10),
                RenderAt = new Location(0, 0, -5),
                DiffuseTexture = Engine.Textures.White
            }, new EntityPhysicsProperty()
            {
                Position = new Location(0, 0, -5),
                Shape = new EntityBoxShape() { Size = new Location(100, 100, 10) },
                Mass = 0
            });
            // Player
            Engine.SpawnEntity(new EntityPhysicsProperty()
            {
                Position = new Location(0, 0, 2),
                Shape = new EntityCharacterShape()
            }, new PlayerEntityControllerCameraProperty());
            // Sky light
            Engine.SpawnEntity(new EntitySkytLight3DProperty());
            // Center light
            Engine.SpawnEntity(new EntityPointLight3DProperty()
            {
                LightPosition = new Location(0, 0, 10),
                LightStrength = 25f
            });
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

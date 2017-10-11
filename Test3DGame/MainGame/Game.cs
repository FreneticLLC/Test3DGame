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
using FreneticGameGraphics.UISystem;
using FreneticGameCore.Files;

namespace Test3DGame.MainGame
{
    /// <summary>
    /// The game's entry point.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The primary backing game client.
        /// </summary>
        public GameClientWindow Client;

        /// <summary>
        /// Starts the game.
        /// </summary>
        public void Start()
        {
            Client = new GameClientWindow(threed: true);
            Client.Engine3D.Forward_Shadows = true;
            Client.Engine3D.MainView.ShadowTexSize = () => 1024;
            Client.OnWindowLoad += Engine_WindowLoad;
            Client.Start();
        }

        /// <summary>
        /// Called by the engine when it loads up.
        /// </summary>
        public void Engine_WindowLoad()
        {
            Client.Window.KeyDown += Window_KeyDown;
            // Ground
            Client.Engine3D.SpawnEntity(new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = Client.Models.Cube,
                Scale = new Location(100, 100, 10),
                RenderAt = new Location(0, 0, -5),
                DiffuseTexture = Client.Textures.White
            }, new ClientEntityPhysicsProperty()
            {
                Position = new Location(0, 0, -5),
                Shape = new EntityBoxShape() { Size = new Location(100, 100, 10) },
                Mass = 0
            });
            // Player
            Client.Engine3D.SpawnEntity(new ClientEntityPhysicsProperty()
            {
                Position = new Location(0, 0, 2),
                Shape = new EntityCharacterShape()
            }, new PlayerEntityControllerCameraProperty());
            // Sky light
            Client.Engine3D.SpawnEntity(new EntitySkyLight3DProperty());
            // Center light
            Client.Engine3D.SpawnEntity(new EntityPointLight3DProperty()
            {
                LightPosition = new Location(0, 0, 10),
                LightStrength = 25f
            });
            UI3DSubEngine subeng = new UI3DSubEngine(new UIPositionHelper(Client.MainUI).Anchor(UIAnchor.TOP_LEFT).ConstantXY(0, 0).ConstantWidthHeight(350, 350));
            Client.MainUI.DefaultScreen.AddChild(subeng);
            // Ground
            subeng.SubEngine.SpawnEntity(new EntitySimple3DRenderableModelProperty()
            {
                EntityModel = Client.Models.Cube,
                Scale = new Location(10, 10, 10),
                RenderAt = new Location(0, 0, -10),
                DiffuseTexture = Client.Textures.White
            });
            // Light
            subeng.SubEngine.SpawnEntity(new EntityPointLight3DProperty()
            {
                LightPosition = new Location(0, 0, 1),
                LightStrength = 15f
            });
            subeng.SubEngine.MainCamera.Position = new Location(0, 0, 0);
            subeng.SubEngine.MainCamera.Direction = new Location(0.1, -0.1, -1).Normalize();
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
                Client.Window.Close();
            }
            if (e.Key == Key.F1)
            {
                SysConsole.Output(OutputType.DEBUG, string.Join("\n", Client.CurrentEngine.EntityList.ConvertAll((ce) => ce.DebugPropList())));
            }
            if (e.Key == Key.F2)
            {
                DataStream ds = new DataStream();
                DataWriter dw = new DataWriter(ds);
                List<string> strs = new List<string>();
                Dictionary<string, int> strmap = new Dictionary<string, int>();
                foreach (ClientEntity ce in Client.CurrentEngine.EntityList)
                {
                    ce.SaveNC(dw, strs, strmap);
                }
                byte[] bStrs = FileHandler.DefaultEncoding.GetBytes(string.Join("\n", strs.ConvertStream((s) => s.Replace("&", "&a").Replace("\n", "&n"))) + "\n\n");
                byte[] b2 = ds.ToArray();
                byte[] bRes = new byte[bStrs.Length + b2.Length];
                bStrs.CopyTo(bRes, 0);
                b2.CopyTo(bRes, bStrs.Length);
                Client.Files.WriteBytes("test.dat", FileHandler.Compress(bRes));
                SysConsole.Output(OutputType.DEBUG, "Saved!");
            }
        }
    }
}

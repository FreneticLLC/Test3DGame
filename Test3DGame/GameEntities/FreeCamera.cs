using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using FreneticGameCore;
using System.Drawing;

namespace Test3DGame.GameEntities
{
    /// <summary>
    /// A simple free-motion camera.
    /// </summary>
    public class FreeCamera : ClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseMove += Window_MouseMove;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDeSpawn()
        {
            Engine.Window.MouseMove -= Window_MouseMove;
        }

        /// <summary>
        /// Current view yaw.
        /// </summary>
        public double Yaw;

        /// <summary>
        /// Current view pitch.
        /// </summary>
        public double Pitch;

        /// <summary>
        /// Mouse sensitivity.
        /// </summary>
        public double Sensitivity = 0.1;
        
        /// <summary>
        /// Tracks motion of the mouse.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            Vector2 newmouse = new Vector2(e.X, e.Y);
            float xm = newmouse.X - (Engine.Window.Width / 2);
            float ym = newmouse.Y - (Engine.Window.Height / 2);
            float adj = Math.Abs(xm) + Math.Abs(ym);
            if (adj >= 1)
            {
                Point pt = Engine.Window.PointToScreen(new Point(Engine.Window.Width / 2, Engine.Window.Height / 2));
                Mouse.SetPosition(pt.X, pt.Y);
                if (adj < 400)
                {
                    Yaw -= xm * Sensitivity;
                    Pitch -= ym * Sensitivity;
                    while (Yaw < 0)
                    {
                        Yaw += 360;
                    }
                    while (Yaw >= 360)
                    {
                        Yaw -= 360;
                    }
                    if (Pitch < -89.9)
                    {
                        Pitch = -89.9;
                    }
                    if (Pitch > 89.9)
                    {
                        Pitch = 89.9;
                    }
                    Engine3D.MainCamera.Direction = Utilities.ForwardVector_Deg(Yaw, Pitch);
                }
            }
        }
    }
}

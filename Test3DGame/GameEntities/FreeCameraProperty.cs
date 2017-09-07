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
    public class FreeCameraProperty : ClientEntityProperty
    {
        /// <summary>
        /// Fired when entity is spawned.
        /// </summary>
        public override void OnSpawn()
        {
            Engine.Window.MouseMove += Window_MouseMove;
            Engine.Window.KeyDown += Window_KeyDown;
            Engine.Window.KeyUp += Window_KeyUp;
            Entity.OnTick += Tick;
        }

        /// <summary>
        /// Fired when entity is despawned.
        /// </summary>
        public override void OnDespawn()
        {
            Engine.Window.MouseMove -= Window_MouseMove;
            Engine.Window.KeyDown -= Window_KeyDown;
            Engine.Window.KeyUp -= Window_KeyUp;
            Entity.OnTick -= Tick;
        }
        
        /// <summary>
        /// Ticks the entity.
        /// </summary>
        public void Tick()
        {
            Location motion = Location.Zero;
            if (KeyForward)
            {
                motion += Engine3D.MainCamera.Direction;
            }
            if (KeyBack)
            {
                motion -= Engine3D.MainCamera.Direction;
            }
            if (KeyRight)
            {
                motion += Engine3D.MainCamera.Side;
            }
            if (KeyLeft)
            {
                motion -= Engine3D.MainCamera.Side;
            }
            if (KeyUp)
            {
                motion += Engine3D.MainCamera.Up;
            }
            if (KeyDown)
            {
                motion -= Engine3D.MainCamera.Up;
            }
            if (motion.LengthSquared() > 0)
            {
                motion.Normalize();
                if (KeyFast)
                {
                    motion *= 5;
                }
                Engine3D.MainCamera.Position += motion * Engine.Delta;
            }

        }

        /// <summary>
        /// Is the left key down.
        /// </summary>
        public bool KeyLeft;

        /// <summary>
        /// Is the right key down.
        /// </summary>
        public bool KeyRight;

        /// <summary>
        /// Is the forward key down.
        /// </summary>
        public bool KeyForward;

        /// <summary>
        /// Is the back key down.
        /// </summary>
        public bool KeyBack;

        /// <summary>
        /// Is the up key down.
        /// </summary>
        public bool KeyUp;

        /// <summary>
        /// Is the down key down.
        /// </summary>
        public bool KeyDown;

        /// <summary>
        /// Is the 'fast' key down.
        /// </summary>
        public bool KeyFast;

        /// <summary>
        /// Tracks key releases.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    KeyForward = false;
                    break;
                case Key.A:
                    KeyLeft = false;
                    break;
                case Key.S:
                    KeyBack = false;
                    break;
                case Key.D:
                    KeyRight = false;
                    break;
                case Key.Q:
                    KeyUp = false;
                    break;
                case Key.E:
                    KeyDown = false;
                    break;
                case Key.LShift:
                    KeyFast = false;
                    break;
            }
        }

        /// <summary>
        /// Tracks key presses.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event data.</param>
        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    KeyForward = true;
                    break;
                case Key.A:
                    KeyLeft = true;
                    break;
                case Key.S:
                    KeyBack = true;
                    break;
                case Key.D:
                    KeyRight = true;
                    break;
                case Key.Q:
                    KeyUp = true;
                    break;
                case Key.E:
                    KeyDown = true;
                    break;
                case Key.LShift:
                    KeyFast = true;
                    break;
            }
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

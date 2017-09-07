using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using OpenTK;
using OpenTK.Input;
using FreneticGameCore;
using FreneticGameCore.EntitySystem;
using System.Drawing;
using BEPUphysics.Character;
using FreneticGameCore.EntitySystem.PhysicsHelpers;
using Test3DGame.GameEntities.GameInterfaces;

namespace Test3DGame.GameEntities
{
    /// <summary>
    /// Represents a player entity controller + camera property.
    /// </summary>
    class PlayerEntityControllerCameraProperty : ClientEntityProperty
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
            Entity.OnSpawnEvent.AddEvent(OnSpawnSecond, this, 0);
            Engine.Window.MouseDown += Window_MouseDown;
        }

        /// <summary>
        /// The physics character controller.
        /// </summary>
        public CharacterController PhysChar;

        /// <summary>
        /// Fired on the secondary spawn event.
        /// </summary>
        /// <param name="e">Event data.</param>
        public void OnSpawnSecond(FreneticEventArgs<EntitySpawnEventArgs> e)
        {
            PhysChar = Entity.GetProperty<ClientEntityPhysicsProperty>().OriginalObject as CharacterController;
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
            Entity.OnSpawnEvent.RemoveBySource(this);
            Engine.Window.MouseDown -= Window_MouseDown;
        }
        
        /// <summary>
        /// Fired when a mouse button is pressed.
        /// </summary>
        /// <param name="sender">The sending object.</param>
        /// <param name="e">Event data.</param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                Engine.SpawnEntity(new EntitySimple3DRenderableModelProperty()
                {
                    EntityModel = Engine.Models.Cube,
                    Scale = new Location(1, 1, 1),
                    DiffuseTexture = Engine.Textures.White,
                    RenderAt = EyePos
                }, new ClientEntityPhysicsProperty()
                {
                    Position = EyePos + new Location(PhysChar.ViewDirection) * 2,
                    LinearVelocity = new Location(PhysChar.ViewDirection * 10),
                    Shape = new EntityBoxShape() { Size = new Location(1, 1, 1) },
                    Mass = 1
                }, new BoxProperty());
            }
            else if (e.Button == MouseButton.Right)
            {
                ClientEntity ent = Engine.SpawnEntity(new EntityPointLight3DProperty()
                {
                    LightColor = new Location(1, 0.2, 0.1),
                    LightPosition = EyePos,
                    LightStrength = 5,
                }, new ClientEntityPhysicsProperty()
                {
                    Position = EyePos + new Location(PhysChar.ViewDirection) * 2,
                    LinearVelocity = new Location(PhysChar.ViewDirection * 7),
                    Shape = new EntitySphereShape() { Size = 0.5 },
                    Mass = 0.5
                });
                ent.GetProperty<EntityPointLight3DProperty>().InternalLight.SetCastShadows(false);
                Engine.Sounds.Play(Engine.Sounds.GetSound("sfx/test"), false, Entity.LastKnownPosition);
            }
        }

        /// <summary>
        /// Gets the player's eye position.
        /// </summary>
        public Location EyePos
        {
            get
            {
                return new Location(PhysChar.Body.Position - PhysChar.Down * PhysChar.StanceManager.StandingHeight * 0.95);
            }
        }

        /// <summary>
        /// Ticks the entity.
        /// </summary>
        public void Tick()
        {
            BEPUutilities.Vector2 motion = BEPUutilities.Vector2.Zero;
            if (KeyForward)
            {
                motion.Y += 1;
            }
            if (KeyBack)
            {
                motion.Y -= 1;
            }
            if (KeyRight)
            {
                motion.X += 1;
            }
            if (KeyLeft)
            {
                motion.X -= 1;
            }
            if (motion.LengthSquared() > 0)
            {
                motion.Normalize();
            }
            PhysChar.ViewDirection = Engine3D.MainCamera.Direction.ToBVector();
            PhysChar.HorizontalMotionConstraint.MovementDirection = motion;
            if (KeyJump)
            {
                PhysChar.Jump();
            }
            if (KeyUse)
            {
                ClientEntity ce = Engine3D.PhysicsWorld.RayTraceSingle(EyePos, Engine3D.MainCamera.Direction, 10, (e) => e.Tag != this);
                ce?.SignalAllInterfacedProperties<IUseable>((e) => e.Use());
            }
            Engine3D.MainCamera.Position = EyePos;
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
        /// Is the jump key down.
        /// </summary>
        public bool KeyJump;

        /// <summary>
        /// Is the use key down.
        /// </summary>
        public bool KeyUse;
        
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
                case Key.Space:
                    KeyJump = false;
                    break;
                case Key.E:
                    KeyUse = false;
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
                case Key.Space:
                    KeyJump = true;
                    break;
                case Key.E:
                    KeyUse = true;
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

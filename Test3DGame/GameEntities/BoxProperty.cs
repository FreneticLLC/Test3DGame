using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreneticGameCore;
using FreneticGameGraphics.ClientSystem.EntitySystem;
using Test3DGame.GameEntities.GameInterfaces;

namespace Test3DGame.GameEntities
{
    /// <summary>
    /// Represents a box in the world.
    /// </summary>
    public class BoxProperty : ClientEntityProperty, IUseable
    {
        /// <summary>
        /// The physics property.
        /// </summary>
        public ClientEntityPhysicsProperty PhysProp;

        public override void OnSpawn()
        {
            Entity.OnTick += Tick;
        }

        public override void OnDespawn()
        {
            Entity.OnTick -= Tick;
        }

        public double scale = 1;

        public bool mode = true;

        public void Tick()
        {
            if (mode)
            {
                scale -= Entity.Engine.Delta;
                if (scale < 0.1)
                {
                    mode = false;
                    scale = 0.1;
                }
            }
            else
            {
                scale += Entity.Engine.Delta;
                if (scale > 1)
                {
                    mode = true;
                    scale = 1;
                }
            }
            Entity.GetProperty<EntitySimple3DRenderableModelProperty>().Scale = new Location(scale);
        }

        /// <summary>
        /// Flings the box into the air.
        /// </summary>
        public void Use()
        {
            if (PhysProp == null)
            {
                PhysProp = Entity.GetProperty<ClientEntityPhysicsProperty>();
            }
            PhysProp.LinearVelocity += new Location(0, 0, 5);
        }
    }
}

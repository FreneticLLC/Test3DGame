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

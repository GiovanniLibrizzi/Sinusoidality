using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    class BaseSystem<T> where T : Component {
        protected static List<T> components = new List<T>();
        public static void Register(T component) {
            components.Add(component);
        }

        public static void Update(GameTime gameTime) {
            foreach (T component in components) {
                component.Update(gameTime);
            }
        }
    }

    class TransformSystem : BaseSystem<Transform> { }
    class SpriteSystem : BaseSystem<Sprite> { }
    class VelocitySystem : BaseSystem<Velocity> { }
    //class ColliderSystem : BaseSystem<Collider> { }
}

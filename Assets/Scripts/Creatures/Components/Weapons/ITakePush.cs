using UnityEngine;

namespace Creatures.Weapons
{
    public interface ITakePush
    {
        void TakePush(Vector2 direction, float force = 1, PushMode pushMode = PushMode.Local);
    }
}
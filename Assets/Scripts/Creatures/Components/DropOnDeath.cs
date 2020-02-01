using Additions.Serializables;

using UnityEngine;

namespace Creatures
{
    public class DropOnDeath : MonoBehaviour, IDie
    {
        [SerializeField, Tooltip("Possible GameObjects to drop.")]
        private WeightedGameObjects dropsTable;

        void IDie.Die(bool suicide)
        {
            if (dropsTable.TryGetRandomElement(out GameObject result))
                Instantiate(result, transform.position, Quaternion.identity);
        }
    }
}
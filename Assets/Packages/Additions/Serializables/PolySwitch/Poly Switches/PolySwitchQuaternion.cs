using UnityEngine;

namespace Additions.Serializables.PolySwitcher
{
    [CreateAssetMenu(fileName = nameof(PolySwitchQuaternion), menuName = nameof(PolySwitcher) + "/Types/" + nameof(PolySwitchQuaternion))]
    public class PolySwitchQuaternion : PolySwitch<Quaternion> { }
}
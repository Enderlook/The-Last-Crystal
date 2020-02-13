using UnityEngine;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [CreateAssetMenu(fileName = nameof(ShortPolySwitchGet), menuName = nameof(Atom) + "/Variables/Others/PolySwitch/" + "Short")]
    public class ShortPolySwitchGet : AtomPolySwitchGetter<short> { }
}
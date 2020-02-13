using Additions.Serializables.PolySwitcher;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    public abstract class AtomPolySwitchGetter<T> : AtomGetter<PolySwitch<T>, T>
    {
        public override T Value => value.Value;
    }
}
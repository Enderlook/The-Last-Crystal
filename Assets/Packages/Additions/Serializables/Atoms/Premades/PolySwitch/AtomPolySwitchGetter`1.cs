using Additions.Serializables.PolySwitcher;

using System;

namespace Additions.Serializables.Atoms.Premades.PolySwitch
{
    [Serializable]
    public abstract class AtomPolySwitchGetter<T> : AtomGetter<PolySwitch<T>, T>
    {
        public override T Value => value.Value;
    }
}
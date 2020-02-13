namespace Additions.Serializables.Atoms
{
    public abstract class AtomGetterWrapper<T, U> : AtomGetter<T, U> where T : IGet<U>
    {
        /// <summary>
        /// <see cref="value"/> as property.
        /// </summary>
        public override U Value => value.Value;
    }
}
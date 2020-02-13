namespace Additions.Serializables.Atoms
{
    public abstract class AtomVariable<T> : AtomConstant<T>, IGetSet<T>
    {
        /// <inheritdoc cref="AtomConstant{T}.Value"/>
        public new T Value {
            get => value;
            set => this.value = value;
        }

        /// <inheritdoc cref="AtomConstant{T}.value"/>
        public new object ObjectValue {
            get => Value;
            set => Value = (T)value;
        }
    }
}
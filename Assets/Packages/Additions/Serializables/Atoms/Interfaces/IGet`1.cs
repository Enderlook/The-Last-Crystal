namespace Additions.Serializables.Atoms
{
    public interface IGet<T> : IGet
    {
        T Value { get; }
    }
}
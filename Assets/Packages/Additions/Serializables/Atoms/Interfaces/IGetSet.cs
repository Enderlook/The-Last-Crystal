namespace Additions.Serializables.Atoms
{
    public interface IGetSet : IGet
    {
        new object ObjectValue { set; }
    }
}
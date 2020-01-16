namespace Additions.Utils
{
    public interface IInitialize
    {
        /// <summary>
        /// Initializes behavior of this object. Useful for object that can't have constructors.
        /// </summary>
        void Initialize();
    }
}

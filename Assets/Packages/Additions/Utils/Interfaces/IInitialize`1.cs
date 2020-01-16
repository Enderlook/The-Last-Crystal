namespace Additions.Utils
{
    public interface IInitialize<in T>
    {
        /// <summary>
        /// Initializes behavior of this object. Useful for object that can't have constructors.
        /// </summary>
        /// <param name="initializer">Parameter used for initializing.</param>
        void Initialize(T initializer);
    }
}
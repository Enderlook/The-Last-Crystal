using UnityEngine;

namespace Additions.Utils
{
    public interface IPrototypable<T> where T : ScriptableObject, IPrototypable<T>
    {
        /// <summary>
        /// Uses this instance as "template" to create new instances of the same class.<br/>
        /// It's not full deep clone, but rather a clone of only the necessary values to make a new instance.<br/>
        /// All other values are left as default.<br/>
        /// This method is useful to use an <see cref="ScriptableObject"/> instance as a template to make new instances of it.
        /// </summary>
        /// <returns>New instance based of this template instance.</returns>
        T CreatePrototype();
    }
}
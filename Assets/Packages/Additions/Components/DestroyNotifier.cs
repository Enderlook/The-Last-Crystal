using System;

using UnityEngine;

namespace Additions.Components
{
    public class DestroyNotifier : MonoBehaviour
    {
        private Action callback;

        public void AddCallback(Action onDeath) => callback += onDeath;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnDestroy() => callback();

        public static void ExecuteOnDeath(GameObject gameObject, Action onDeath) => (gameObject.GetComponent<DestroyNotifier>() ?? gameObject.AddComponent<DestroyNotifier>()).AddCallback(onDeath);
    }
}
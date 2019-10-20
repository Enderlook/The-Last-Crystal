using System;
using UnityEngine;

namespace AdditionalComponents
{
    public class DestroyNotifier : MonoBehaviour
    {
        private Action callback;

        public void AddCallback(Action onDeath) => callback += onDeath;

        private void OnDestroy() => callback();

        public static void ExecuteOnDeath(GameObject gameObject, Action onDeath) => (gameObject.GetComponent<DestroyNotifier>() ?? gameObject.AddComponent<DestroyNotifier>()).AddCallback(onDeath);
    }
}
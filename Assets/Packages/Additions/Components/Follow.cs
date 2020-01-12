using UnityEngine;

namespace Additions.Components
{
    public class Follow : MonoBehaviour
    {
#pragma warning disable CS0649
        [Header("Build")]
        [SerializeField, Tooltip("Following transform")]
        private Transform transformToFollow;
#pragma warning restore CS0649

        private Vector3 originalLocalPosition;

        /* TODO:
         * https://forum.unity.com/threads/exposing-fields-with-interface-type-c-solved.49524/
         * https://forum.unity.com/threads/c-interface-wont-show-in-inspector.383886/
         * https://forum.unity.com/threads/understanding-iserializationcallbackreceiver.383757/
         */

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake() => originalLocalPosition = transform.localPosition;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        // Do? https://forum.unity.com/threads/subtracting-quaternions.317649/
        private void LateUpdate() => transform.localPosition = originalLocalPosition + transformToFollow.localPosition;
    }
}
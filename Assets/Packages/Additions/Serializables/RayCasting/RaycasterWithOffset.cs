using System;

using UnityEngine;

namespace Additions.Serializables.Physics
{
    /// <summary>
    /// Used to perform raycast with predefined parameters.<br>
    /// It allows adding offset to the <see cref="Raycaster.Source"/> on runtime without using <see cref="Raycaster.SetReference(Transform, SpriteRenderer)"/> or <see cref="Raycaster.SetReference(Vector2, SpriteRenderer)"/>.<br>
    /// It can be either serialized in Unity inspector or construct using new.
    /// </summary>
    [Serializable]
    public class RaycasterWithOffset : Raycaster
    {
        [NonSerialized]
        private Vector2 offset;

        protected override Vector2 Reference => (referenceTransform == null ? referenceVector : (Vector2)referenceTransform.position) + (offset == null ? Vector2.zero : offset);

        public void ResetOffset() => offset = Vector2.zero;

        public void SetOffset(Vector2 offset) => this.offset = offset;

        public Vector2 GetOffset() => offset;
    }
}
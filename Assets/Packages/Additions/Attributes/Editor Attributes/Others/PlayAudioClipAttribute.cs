using Additions.Attributes.AttributeUsage;

using System;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageRequireDataType(typeof(AudioClip), includeEnumerableTypes = true)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class PlayAudioClipAttribute : PropertyAttribute
    {
        public bool ShowProgressBar { get; }

        public PlayAudioClipAttribute(bool showProgressBar) => ShowProgressBar = showProgressBar;

        public PlayAudioClipAttribute() { }
    }
}
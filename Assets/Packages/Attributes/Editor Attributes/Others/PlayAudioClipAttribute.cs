using AdditionalAttributes.AttributeUsage;

using System;

using UnityEngine;

namespace AdditionalAttributes
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
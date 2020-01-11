using AdditionalAttributes;

using System;

using UnityEngine;

using Utils;

namespace ScriptableSound.Modifiers
{
    [AbstractScriptableObject]
    public class SoundModifier : ScriptableObject, IPrototypable<SoundModifier>
    {
        public virtual void ModifyAudioSource(AudioSource audioSource) => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");

        public virtual void BackToNormalAudioSource(AudioSource audioSource) => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");

        public virtual SoundModifier CreatePrototype() => throw new NotImplementedException("This class is 'abstract' and should not be instantiated by its own. Use derived classes instead which override this method.");

#if UNITY_EDITOR
        public virtual void Validate(SoundClip soundClip) { }
#endif
    }
}
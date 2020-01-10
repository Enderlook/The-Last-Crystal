using System.Reflection;

using UnityEngine;

using Utils;

namespace ScriptableSound.Modifiers
{
    [CreateAssetMenu(fileName = "StartAt", menuName = "Sound/Modifiers/Start At")]
    public class StartAtModifier : SoundModifier
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Start sound at second.")]
        private float startAtSecond;
#pragma warning restore CS0649

        private float oldValue;

        private FieldInfo audioClip = typeof(SoundClip).GetField("audioclip");

        public override void ModifyAudioSource(AudioSource audioSource)
        {
            oldValue = audioSource.time;
            audioSource.time = startAtSecond; // Note: this only works if audiosource.Play() was called before
        }

        public override void BackToNormalAudioSource(AudioSource audioSource) => audioSource.time = oldValue;

        public override SoundModifier CreatePrototype()
        {
            StartAtModifier prototype = CreateInstance<StartAtModifier>();
            prototype.name = PrototypeHelper.GetPrototypeNameOf(prototype);
            prototype.startAtSecond = startAtSecond;
            return prototype;
        }

#if UNITY_EDITOR
        public override void Validate(SoundClip soundClip)
        {
            if (((AudioClip)audioClip.GetValue(soundClip))?.length < startAtSecond)
                Debug.LogError($"Modifier {name} ({nameof(StartAtModifier)}) in {soundClip.name} ({nameof(SoundClip)}) can't have a {nameof(startAtSecond)} field value greater than the length of the {nameof(AudioClip)} that is modifying.");
            base.Validate(soundClip);
        }
#endif
    }
}
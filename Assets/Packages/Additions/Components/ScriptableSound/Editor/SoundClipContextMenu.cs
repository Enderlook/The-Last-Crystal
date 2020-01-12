using Additions.Utils.UnityEditor;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Additions.Components.ScriptableSound
{
    internal static class SoundClipContextMenu
    {
        [MenuItem("Assets/Sound/Sound Clip/Create")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void CreateSoundClipsOneByOne()
        {
            AudioClip[] audioClips = Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets).ToArray();

            foreach ((SoundClip soundClip, AudioClip audioClip) in CreateSoundClips(audioClips).Zip(audioClips, (s, a) => (s, a)))
                AssetDatabaseHelper.CreateAsset(soundClip,
                    string.Join(".", AssetDatabase.GetAssetPath(audioClip)
                        .Split('.').Reverse().Skip(1).Reverse().Append("asset").ToArray()));
        }

        [MenuItem("Assets/Sound/Sound Clip/Create all in a single asset")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void CreateSoundClipsInSingleAsset()
        {
            AudioClip[] audioClips = Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets).ToArray();

            AssetDatabaseHelper.CreateAssetFromObjects(
                CreateSoundClips(audioClips).ToArray(),
                Path.Combine(AssetDatabaseHelper.GetAssetDirectory(audioClips[0]), "SoundClip.asset"), true);
        }

        public static IEnumerable<SoundClip> CreateSoundClips(IEnumerable<AudioClip> audioClips) => audioClips.Select(CreateSoundClip);

        private static SoundClip CreateSoundClip(AudioClip audioClip)
        {
            SoundClip soundClip = ScriptableObject.CreateInstance<SoundClip>();
            soundClip.name = audioClip.name;

            soundClip.GetType().GetField("audioClip", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).SetValue(soundClip, audioClip);

            return soundClip;
        }

        [MenuItem("Assets/Sound/Create Sound Clip", true), MenuItem("Assets/Sound/Sound Clip/Create all in a single asse", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static bool CreateSoundClipsValidation() => Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets).Length > 0;
    }
}
using Additions.Utils.UnityEditor;

using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Additions.Components.ScriptableSound
{
    internal class SoundListContextMenu : MonoBehaviour
    {
        [MenuItem("Assets/Sound/Sound List/Create from SoundClips")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void CreateSoundListFromSoundClips()
        {
            SoundClip[] soundClips = Selection.GetFiltered<SoundClip>(SelectionMode.DeepAssets);
            SoundList soundList = CreateSoundList(soundClips);

            AssetDatabaseHelper.CreateAsset(soundList,
                Path.Combine(AssetDatabaseHelper.GetAssetDirectory(soundClips[0]), "SoundList.asset"));
        }

        [MenuItem("Assets/Sound/Create Sound List", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static bool CreateSoundListFromSoundClipsValidation() => Selection.GetFiltered<SoundClip>(SelectionMode.DeepAssets).Length > 0;

        [MenuItem("Assets/Sound/Sound List/Create from AudioClips")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]

        private static void CreateSoundListFromAudioClips()
        {
            AudioClip[] audioClips = Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets);

            SoundClip[] soundClips = SoundClipContextMenu.CreateSoundClips(audioClips).ToArray();

            SoundList soundList = CreateSoundList(soundClips);

            string path = AssetDatabaseHelper.CreateAsset(soundList,
                Path.Combine(AssetDatabaseHelper.GetAssetDirectory(audioClips[0]), "SoundList.asset"));

            AssetDatabaseHelper.AddObjectToAsset(soundClips, path);
        }

        [MenuItem("Assets/Sound/Sound List/Create from AudioClips", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static bool CreateSoundListFromAudioClipsValidation() => Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets).Length > 0;

        private static SoundList CreateSoundList(SoundClip[] soundClips)
        {
            SoundList soundList = ScriptableObject.CreateInstance<SoundList>();

            soundList.GetType().GetField("sounds", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(soundList, soundClips);

            return soundList;
        }

        [MenuItem("Assets/Sound/Sound List/Create from AudioClips and SoundClips", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void CreateSoundListFromAudioClipsAndSoundClips()
        {
            AudioClip[] audioClips = Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets);

            SoundClip[] soundClipsCreated = SoundClipContextMenu.CreateSoundClips(audioClips).ToArray();

            SoundClip[] soundClipSelected = Selection.GetFiltered<SoundClip>(SelectionMode.DeepAssets);

            SoundList soundList = CreateSoundList(soundClipsCreated.Concat(soundClipSelected).ToArray());

            string path = AssetDatabaseHelper.CreateAsset(soundList,
                Path.Combine(AssetDatabaseHelper.GetAssetDirectory(audioClips[0]), "SoundList.asset"));

            AssetDatabaseHelper.AddObjectToAsset(soundClipSelected, path);
        }

        [MenuItem("Assets/Sound/Sound List/Create from AudioClips and SoundClips", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static bool CreateSoundListFromAudioClipsAndSoundClipsValidation() => Selection.GetFiltered<AudioClip>(SelectionMode.DeepAssets).Length > 0
            && Selection.GetFiltered<SoundClip>(SelectionMode.DeepAssets).Length > 0;

    }
}
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace ScriptableSound
{
    public class SoundListContextMenu : MonoBehaviour
    {
        [MenuItem("Assets/Create Sound List")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void CreateSoundList()
        {
            SoundClip[] soundClips = Selection.GetFiltered<SoundClip>(SelectionMode.DeepAssets);
            SoundList soundList = ScriptableObject.CreateInstance<SoundList>();

            soundList.GetType().GetField("sounds", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(soundList, soundClips);

            AssetDatabaseHelper.CreateAsset(soundList,
                Path.Combine(AssetDatabaseHelper.GetAssetDirectory(soundClips[0]), "SoundList.asset"));
        }

        [MenuItem("Assets/Create Sound List", true)]
        private static bool CreateSoundListValidation() => Selection.objects.Any(e => e.GetType() == typeof(SoundClip));
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
    }
}
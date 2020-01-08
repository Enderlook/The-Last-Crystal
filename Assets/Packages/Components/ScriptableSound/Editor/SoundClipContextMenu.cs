using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace ScriptableSound
{
    public static class SoundClipContextMenu
    {
        [MenuItem("Assets/Create Sound Clip")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void CreateSoundClip()
        {
            AudioClip audioClip = (AudioClip)Selection.activeObject;
            SoundClip soundClip = ScriptableObject.CreateInstance<SoundClip>();

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy; ;

            soundClip.GetType().GetField("audioClip", bindingFlags).SetValue(soundClip, audioClip);

            AssetDatabaseHelper.SaveAsset(soundClip,
                string.Join(".", AssetDatabase.GetAssetPath(audioClip)
                    .Split('.').Reverse().Skip(1)
                    .Reverse().Append(".asset").ToArray()));
        }

        [MenuItem("Assets/Create Sound Clip", true)]
        private static bool CreateSoundClipValidation() => Selection.activeObject is AudioClip;
    }
}
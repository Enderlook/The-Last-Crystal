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

            // We do this after store the asset so Unity initialize and serialize all the fields for us

            object volume = typeof(Sound).GetField("volume", bindingFlags).GetValue(soundClip);
            Type switchType = volume.GetType().BaseType.BaseType;
            switchType.GetField("useAlternative", bindingFlags).SetValue(volume, true);
            switchType.GetField("item2", bindingFlags).SetValue(volume, 1);

            object pitch = typeof(Sound).GetField("pitch", bindingFlags).GetValue(soundClip);
            switchType.GetField("useAlternative", bindingFlags).SetValue(pitch, true);
            switchType.GetField("item2", bindingFlags).SetValue(pitch, 1);

        }

        [MenuItem("Assets/Create Sound Clip", true)]
        private static bool CreateSoundClipValidation() => Selection.activeObject is AudioClip;
    }
}
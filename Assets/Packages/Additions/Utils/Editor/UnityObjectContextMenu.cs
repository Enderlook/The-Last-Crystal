using UnityEditor;

using UnityObject = UnityEngine.Object;

namespace Additions.Utils.UnityEditor
{
    internal static class UnityObjectContextMenu
    {
        [MenuItem("Assets/Extract Sub-Assets")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static void ExtractSubAsset()
        {
            foreach (UnityObject selection in Selection.objects)
            {
                UnityObject subAsset = selection;
                AssetDatabaseHelper.ExtractSubAsset(ref subAsset);
            }
        }

        [MenuItem("Assets/Extract Sub-Assets", true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private static bool ExtractSubAssetValidator()
        {
            foreach (UnityObject selection in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(selection);
                if (AssetDatabase.LoadMainAssetAtPath(path) != selection)
                    return true;
            }
            return false;
        }
    }
}
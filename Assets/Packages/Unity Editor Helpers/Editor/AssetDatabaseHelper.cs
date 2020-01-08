using System.IO;

using UnityEditor;

using UnityObject = UnityEngine.Object;

namespace UnityEditorHelper
{
    public static class AssetDatabaseHelper
    {
        /// <summary>
        /// Save asset to path, creating the necessaries directories.</br>
        /// It automatically add "Assets/" to the <paramref name="path"/> if it doesn't have.
        /// </summary>
        /// <param name="asset">Asset to save.</param>
        /// <param name="path">Path to save file</param>
        public static void SaveAsset(UnityObject asset, string path)
        {
            path = path.StartsWith("Assets/") ? path : "Assets/" + path;
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.Refresh();
        }
    }
}
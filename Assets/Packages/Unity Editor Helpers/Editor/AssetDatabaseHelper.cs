using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityObject = UnityEngine.Object;

namespace UnityEditorHelper
{
    public static class AssetDatabaseHelper
    {
        private static string GetPathFromAssets(string path)
        {
            path = path.Replace('\\', '/');
            return path.StartsWith("Assets/") || path.StartsWith("Assets//") ? path : "Assets/" + path;
        }

        private static void CreateDirectoryIfDoesNotExist(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Save asset to path, creating the necessaries directories.</br>
        /// It automatically add "Assets/" to the <paramref name="path"/> if it doesn't have.
        /// </summary>
        /// <param name="asset">Asset to save.</param>
        /// <param name="path">Path to save file</param>
        /// <param name="generateUniquePath">If <see language="true"/> it will change the name of file to avoid name collision.</param>
        /// <return>Path to created file.</return>
        public static string CreateAsset(UnityObject asset, string path, bool generateUniquePath = false)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Length == 0) throw new ArgumentException("Can't be empty", nameof(path));

            path = GetPathFromAssets(path);
            CreateDirectoryIfDoesNotExist(path);
            if (generateUniquePath)
                path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            return path;
        }

        /// <summary>
        /// Save asset to path, creating the necessaries directories.</br>
        /// All assets are stored in the same file.<br/>
        /// It automatically add "Assets/" to the <paramref name="path"/> if it doesn't have.
        /// </summary>
        /// <param name="objects">Assets to save.</param>
        /// <param name="path">Path to save file</param>
        /// <param name="generateUniquePath">If <see language="true"/> it will change the name of file to avoid name collision.</param>
        /// <return>Path to created file.</return>
        public static void CreateAssetFromObjects(IEnumerable<UnityObject> objects, string path, bool generateUniquePath = false)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Length == 0) throw new ArgumentException("Can't be empty", nameof(path));

            path = CreateAsset(objects.First(), path, generateUniquePath);
            foreach (UnityObject @object in objects.Skip(1))
            {
                AssetDatabase.AddObjectToAsset(@object, path);
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// Add object to asset in path, creating the necessaries directories.</br>
        /// It automatically add "Assets/" to the <paramref name="path"/> if it doesn't have.
        /// </summary>
        /// <param name="objectToAdd">Asset to add.</param>
        /// <param name="path">Path to save file</param>
        /// <param name="createIfNotExist">If <see language="true"/> it will create the asset if it doesn't exist.</param>
        /// <return>Path to created or modified file.</return>
        public static string AddObjectToAsset(UnityObject objectToAdd, string path, bool createIfNotExist = false)
        {
            if (objectToAdd == null) throw new ArgumentNullException(nameof(objectToAdd));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Length == 0) throw new ArgumentException("Can't be empty", nameof(path));

            path = GetPathFromAssets(path);
            CreateDirectoryIfDoesNotExist(path);

            if (File.Exists(path))
                AssetDatabase.AddObjectToAsset(objectToAdd, path);
            else
            {
                if (createIfNotExist)
                    AssetDatabase.CreateAsset(objectToAdd, path);
                else
                    throw new FileNotFoundException("Not found asset", path);
            }
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            return path;
        }

        /// <summary>
        /// Add objects to asset in path, creating the necessaries directories.</br>
        /// It automatically add "Assets/" to the <paramref name="path"/> if it doesn't have.
        /// </summary>
        /// <param name="objectsToAdd">Objects to add to asset to add.</param>
        /// <param name="path">Path to save file</param>
        /// <param name="createIfNotExist">If <see language="true"/> it will create the asset if it doesn't exist.</param>
        /// <return>Path to created or modified file.</return>
        public static string AddObjectToAsset(IEnumerable<UnityObject> objectsToAdd, string path, bool createIfNotExist = false)
        {
            if (objectsToAdd == null) throw new ArgumentNullException(nameof(objectsToAdd));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Length == 0) throw new ArgumentException("Can't be empty", nameof(path));

            foreach (UnityObject objectToAdd in objectsToAdd)
                path = AddObjectToAsset(objectToAdd, path, createIfNotExist);
            return path;
        }

        /// <summary>
        /// Get directory of the asset <paramref name="object"/>.
        /// </summary>
        /// <param name="object">Asset to get directory.</param>
        /// <returns>Directory where thee asset is saved.</returns>
        public static string GetAssetDirectory(UnityObject @object) => Path.GetDirectoryName(AssetDatabase.GetAssetPath(@object));
    }
}
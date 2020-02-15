﻿using Additions.Attributes.AttributeUsage.PostCompiling;
using Additions.Extensions;
using Additions.Utils.UnityEditor;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace Additions.Attributes
{
    internal class ScriptableObjectWindow : EditorWindow
    {
        private const string DEFAULT_PATH = "Resources/";
        private static ILookup<Type, Type> derivedTypes;

        private SerializedProperty property;
        private Func<object> get;
        private Action<object> set;
        private Type[] allowedTypes;
        private string[] allowedTypesNames;
        private int index;
        private string path = DEFAULT_PATH;
        private string scriptableObjectName;
        private string propertyPath;

        private static void InitializeDerivedTypes()
        {
            Stack<Type> types = new Stack<Type>(AssembliesHelper.GetAllAssembliesOfPlayerAndEditorAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => typeof(ScriptableObject).IsAssignableFrom(e)));

            HashSet<KeyValuePair<Type, Type>> typesKV = new HashSet<KeyValuePair<Type, Type>>();

            while (types.TryPop(out Type result))
            {
                typesKV.Add(new KeyValuePair<Type, Type>(result, result));
                Type baseType = result.BaseType;
                if (typeof(ScriptableObject).IsAssignableFrom(baseType))
                {
                    typesKV.Add(new KeyValuePair<Type, Type>(baseType, result));
                    types.Push(baseType);
                }
            }

            derivedTypes = typesKV.ToLookup();
        }

        private static IEnumerable<Type> GetDerivedTypes(Type type)
        {
            Stack<Type> types = new Stack<Type>(derivedTypes[type].Where(e => e != type));
            LinkedList<Type> linkedList = new LinkedList<Type>(types);
            linkedList.AddFirst(type);

            while (types.TryPop(out Type result))
            {
                foreach (Type t in derivedTypes[result].Where(e => e != result))
                {
                    types.Push(t);
                    linkedList.AddLast(t);
                }
            }
            return linkedList;
        }

        public static void CreateWindow(SerializedProperty property, FieldInfo fieldInfo)
        {
            if (derivedTypes == null)
                InitializeDerivedTypes();

            ScriptableObjectWindow window = GetWindow<ScriptableObjectWindow>();

            window.propertyPath = AssetDatabaseHelper.GetAssetPath(property);
            Type type;
            /* If the property came from an array and the element is null this will be null which is a problem for us.
             * This is also null if the property isn't array but the field is empty (null). That is also a problem. */
            if (property.objectReferenceValue)
            {
                (window.get, window.set) = property.GetTargetObjectAccessors();
                type = property.GetFieldType();
            }
            else
            {
                UnityObject targetObject = property.serializedObject.targetObject;
                Type fieldType = fieldInfo.FieldType;
                // Just confirming that it's an array
                if (fieldType.IsArray)
                {
                    type = fieldType.GetElementType();
                    int index = property.GetIndexFromArray();

                    if (fieldInfo.GetValue(targetObject) is Array array)
                    {
                        /* Until an element is in-Inspector dragged to the array element field, it seems that Unity doesn't rebound the array
                         * So if the array is empty and it doesn't have space for us, we make a new array and inject it. */
                        if (array.Length == 0)
                        {
                            array = Array.CreateInstance(fieldType.GetElementType(), 1);
                            fieldInfo.SetValue(targetObject, array);
                        }

                        window.get = () => array.GetValue(index);
                        window.set = (value) => array.SetValue(value, index);
                    }
                    else
                        throw new InvalidCastException();
                }
                else
                {
                    type = fieldType;
                    window.get = () => property.objectReferenceValue;
                    Action<object, object> set;
                    if (fieldInfo.FieldType == targetObject.GetType())
                        set = fieldInfo.SetValue;
                    else
                    {
                        FieldInfo fieldInfo2 = targetObject
                                .GetType()
                                .GetField(property.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fieldInfo2 != null)
                            set = fieldInfo2.SetValue;
                        else
                            set = (_, value) => property.objectReferenceValue = (UnityObject)value;
                    }
                    window.set = (value) => set(targetObject, value);
                }
            }
            IEnumerable<Type> allowedTypes = GetDerivedTypes(type).Where(e => !e.IsAbstract);

            // RestrictTypeAttribute compatibility
            RestrictTypeAttribute restrictTypeAttribute = fieldInfo.GetCustomAttribute<RestrictTypeAttribute>();
            if (restrictTypeAttribute != null)
                allowedTypes = allowedTypes.Where(e => restrictTypeAttribute.CheckIfTypeIsAllowed(e, out string _)).ToArray();

            window.allowedTypes = allowedTypes.ToArray();

            window.allowedTypesNames = window.allowedTypes.Select(e => e.Name).ToArray();
            window.index = window.GetIndex(type);
            window.property = property;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnGUI()
        {
            titleContent = new GUIContent("Scriptable Object Manager");

            ScriptableObject scriptableObject = (ScriptableObject)get?.Invoke();
            bool hasScriptableObject = scriptableObject != null;

            // Instance Type
            EditorGUI.BeginDisabledGroup(hasScriptableObject);
            if (hasScriptableObject)
                index = GetIndex(scriptableObject.GetType());
            index = EditorGUILayout.Popup(new GUIContent("Instance type", "Scriptable object instance type to create."), index, allowedTypesNames);

            // Path to Scriptable Object
            string pathToAsset = AssetDatabase.GetAssetPath(scriptableObject);
            bool hasAsset = !string.IsNullOrEmpty(pathToAsset);
            path = hasAsset ? pathToAsset : path;
            path = EditorGUILayout.TextField(new GUIContent("Path to file", "Path where the asset file is stored or will be saved."), path);
            string _path = path.StartsWith("Assets/") ? path : "Assets/" + path;
            _path = _path.EndsWith(".asset") ? _path : _path + ".asset";
            if (!hasAsset)
                EditorGUILayout.LabelField("Path to save:", _path);
            EditorGUI.EndDisabledGroup();

            UnityObject targetObject = property.serializedObject.targetObject;

            if (!hasAsset && !hasScriptableObject)
            {
                EditorGUI.BeginDisabledGroup(index == -1);
                // Create
                if (GUILayout.Button(new GUIContent("Instantiate in field and add to asset", "Create and instance and assign to field. The scriptable object will be added to the scene/prefab file.")))
                {
                    scriptableObject = InstantiateAndApply(targetObject);
                    AssetDatabaseHelper.AddObjectToAsset(scriptableObject, propertyPath);
                }

                // Create and Save
                if (GUILayout.Button(new GUIContent("Instantiate in field and save asset", "Create and instance, assign to field and save it as an asset file.")))
                {
                    scriptableObject = InstantiateAndApply(targetObject);
                    AssetDatabaseHelper.CreateAsset(scriptableObject, _path);
                }
                EditorGUI.EndDisabledGroup();
            }

            if (hasScriptableObject)
            {
                // Rename
                if (string.IsNullOrEmpty(scriptableObjectName))
                    scriptableObjectName = scriptableObject.name;
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(scriptableObjectName == scriptableObject.name);
                if (GUILayout.Button(new GUIContent("Rename", "Change the name of Scriptable Object.")))
                {
                    Undo.RecordObject(scriptableObject, "Rename");
                    scriptableObject.name = scriptableObjectName;
                    property.serializedObject.ApplyModifiedProperties();
                }
                EditorGUI.EndDisabledGroup();
                scriptableObjectName = EditorGUILayout.TextField(new GUIContent($"New Name", "Change current name to new one."), scriptableObjectName);

                /// Clean
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button(new GUIContent("Clean field", "Remove current instance of field.")))
                {
                    Undo.RecordObject(targetObject, "Clean field");
                    set(null);
                    path = DEFAULT_PATH;
                    property.serializedObject.ApplyModifiedProperties();
                }

                if (!hasAsset)
                    // Save
                    if (GUILayout.Button(new GUIContent("Save asset as file", "Save instance as an asset file.")))
                        AssetDatabaseHelper.CreateAsset(scriptableObject, _path);
            }
        }

        private ScriptableObject InstantiateAndApply(UnityObject targetObject)
        {
            ScriptableObject scriptableObject;
            Undo.RecordObject(targetObject, "Instantiate field");
            scriptableObject = CreateInstance(allowedTypes[index]);
            set(scriptableObject);
            property.serializedObject.ApplyModifiedProperties();
            return scriptableObject;
        }

        private int GetIndex(Type type) => Array.IndexOf(allowedTypes, type);
    }
}

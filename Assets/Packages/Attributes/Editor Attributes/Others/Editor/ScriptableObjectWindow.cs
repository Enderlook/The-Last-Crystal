﻿using AdditionalExtensions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace AdditionalAttributes
{
    internal class ScriptableObjectWindow : EditorWindow
    {
        private static ILookup<Type, Type> derivedTypes;

        private Func<object> get;
        private Action<object> set;
        private Type[] allowedTypes;
        private string[] allowedTypesNames;
        private int index;
        private string path = "Resources/";

        private static void InitializeDerivedTypes()
        {
            Type[] types = AssembliesHelper.GetAllAssembliesOfPlayerAndEditorAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => e.IsSubclassOf(typeof(ScriptableObject))).ToArray();

            derivedTypes = types
                .Select(e => new KeyValuePair<Type, Type>(e.BaseType, e))
                .Concat(types.Select(e => new KeyValuePair<Type, Type>(e, e)))
                .ToLookup();
        }

        private static IEnumerable<Type> GetDerivedTypes(Type type) => derivedTypes[type].Where(e => e != type).SelectMany(GetDerivedTypes).Prepend(type);

        public static void CreateWindow(SerializedProperty property, FieldInfo fieldInfo)
        {
            if (derivedTypes == null)
                InitializeDerivedTypes();

            ScriptableObjectWindow window = GetWindow<ScriptableObjectWindow>();
            Type type;
            // If the property came from an array and the element is null this will be null which is a problem for us
            if (property.objectReferenceValue)
            {
                (window.get, window.set) = property.GetTargetObjectAccessors();
                type = property.GetFieldType();
            }
            else
            {
                Type fieldType = fieldInfo.FieldType;
                // Just confirming that it's an array
                if (fieldType.IsArray)
                {
                    type = fieldType.GetElementType();
                    int index = property.GetIndexFromArray();
                    if (fieldInfo.GetValue(property.serializedObject.targetObject) is Array array)
                    {
                        window.get = () => array.GetValue(index);
                        window.set = (object value) => array.SetValue(value, index);
                    }
                    else
                        throw new InvalidCastException();
                }
                // Just to be sure, but I think this case is impossible
                else
                {
                    type = fieldType;
                    window.get = () => property.objectReferenceValue;
                    window.set = (object value) => property.objectReferenceValue = (UnityEngine.Object)value;
                }
            }
            window.allowedTypes = GetDerivedTypes(type).ToArray();
            window.allowedTypesNames = window.allowedTypes.Select(e => e.Name).ToArray();
            window.index = Array.IndexOf(window.allowedTypes, type);
        }

        private void OnGUI()
        {
            titleContent = new GUIContent("Scriptable Object Manager");

            index = EditorGUILayout.Popup(new GUIContent("Instance type", "Scriptable object instance type to create."), index, allowedTypesNames);

            path = EditorGUILayout.TextField(new GUIContent("Path to save file", "Path used to save the asset file."), path);
            string _path = path.StartsWith("Assets/") ? path : "Assets/" + path;
            _path = _path.EndsWith(".asset") ? _path : _path + ".asset";
            EditorGUILayout.LabelField("Path", _path);

            ScriptableObject scriptableObject = (ScriptableObject)get?.Invoke();

            if (GUILayout.Button(new GUIContent("Instantiate in field", "Create and instance and assign to field.")))
                set(Create());
            if (GUILayout.Button(new GUIContent("Instantiate in field and save asset", "Create and instance, assign to field and save it as an asset file.")))
            {
                scriptableObject = Create();
                set(scriptableObject);
                Save(scriptableObject, _path);
            }

            EditorGUI.BeginDisabledGroup(scriptableObject == null);
            if (GUILayout.Button(new GUIContent("Clean field", "Remove current instance of field.")))
                set(null);
            if (GUILayout.Button(new GUIContent("Save asset", "Save instance as an asset file.")))
                Save(scriptableObject, _path);
            EditorGUI.EndDisabledGroup();
        }

        private ScriptableObject Create() => CreateInstance(allowedTypes[index]);

        private static void Save(ScriptableObject scriptableObject, string path)
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
            AssetDatabase.CreateAsset(scriptableObject, path);
            AssetDatabase.Refresh();
        }
    }
}

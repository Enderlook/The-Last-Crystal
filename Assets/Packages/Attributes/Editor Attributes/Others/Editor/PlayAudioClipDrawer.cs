using System;
using System.Reflection;

using UnityEditor;

using UnityEditorHelper;

using UnityEngine;

namespace AdditionalAttributes
{
    [CustomPropertyDrawer(typeof(PlayAudioClipAttribute))]
    public class PlayAudioClipDrawer : AdditionalPropertyDrawer
    {
        // https://forum.unity.com/threads/way-to-play-audio-in-editor-using-an-editor-script.132042/
        // https://github.com/MattRix/UnityDecompiled/blob/cc432a3de42b53920d5d5dae85968ff993f4ec0e/UnityEditor/UnityEditor/AudioUtil.cs

        private static readonly Action<AudioClip> Play;
        private static readonly Action<AudioClip> Stop;
        private static readonly Func<AudioClip, bool> IsClipPlaying;
        private static readonly Func<AudioClip, float> GetClipPosition;

        static PlayAudioClipDrawer()
        {
            Type audioUtilClass = typeof(AudioImporter).Assembly.GetType("UnityEditor.AudioUtil");
            Type[] parameter = new Type[] { typeof(AudioClip) };
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public;
            Play = (Action<AudioClip>)audioUtilClass.GetMethod("PlayClip", bindingFlags, null, parameter, null).CreateDelegate(typeof(Action<AudioClip>));
            Stop = (Action<AudioClip>)audioUtilClass.GetMethod("StopClip", bindingFlags, null, parameter, null).CreateDelegate(typeof(Action<AudioClip>));
            IsClipPlaying = (Func<AudioClip, bool>)audioUtilClass.GetMethod("IsClipPlaying", bindingFlags, null, parameter, null).CreateDelegate(typeof(Func<AudioClip, bool>));
            GetClipPosition = (Func<AudioClip, float>)audioUtilClass.GetMethod("GetClipPosition", bindingFlags, null, parameter, null).CreateDelegate(typeof(Func<AudioClip, float>));
        }

        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            PlayAudioClipAttribute playAudioClipAttribute = (PlayAudioClipAttribute)attribute;

            AudioClip audioClip = (AudioClip)property.GetTargetObjectOfProperty();
            bool isPlaying = IsClipPlaying(audioClip);
            GUIContent playGUIContent = isPlaying ? EditorGUIUtility.IconContent("PauseButton", $"Stop {typeof(AudioClip)}. Remaining duration: {audioClip.length - GetClipPosition(audioClip)} seconds.") : EditorGUIUtility.IconContent("PlayButton", $"Play {typeof(AudioClip)}. Duration: {audioClip.length} seconds.");
            float width = GUI.skin.button.CalcSize(playGUIContent).x;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - width, position.height), property, label, true);

            bool showProgressBar = isPlaying && playAudioClipAttribute.ShowProgressBar;

            if (GUI.Button(new Rect(position.x + position.width - width, position.y, width, position.height / (showProgressBar ? 2 : 1)), playGUIContent))
            {
                if (isPlaying)
                    Stop(audioClip);
                else
                    Play(audioClip);
            }

            if (showProgressBar)
            {
                EditorGUI.ProgressBar(new Rect(position.x + position.width - width, position.y + position.height / 2, width, position.height / 2), GetClipPosition(audioClip) / audioClip.length, "");
                // Forces repaint all the inspector per frame
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }
    }
}
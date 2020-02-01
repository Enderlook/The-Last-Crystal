using Additions.Utils;
using Additions.Utils.UnityEditor;

using System;

using UnityEditor;

using UnityEngine;

namespace Additions.Attributes
{
    [CustomPropertyDrawer(typeof(PlayAudioClipAttribute))]
    public class PlayAudioClipDrawer : AdditionalPropertyDrawer
    {
        protected override void OnGUIAdditional(Rect position, SerializedProperty property, GUIContent label)
        {
            PlayAudioClipAttribute playAudioClipAttribute = (PlayAudioClipAttribute)attribute;

            AudioClip audioClip = (AudioClip)property.GetTargetObjectOfProperty();
            if (audioClip == null)
                EditorGUI.PropertyField(position, property, label);
            else
            {
                bool isPlaying = AudioUtil.IsClipPlaying(audioClip);
                GUIContent playGUIContent = EditorGUIUtility.IconContent(isPlaying ? "PauseButton" : "PlayButton");
                float width = GUI.skin.button.CalcSize(playGUIContent).x;

                playGUIContent.tooltip = TimeSpan.FromSeconds(isPlaying
                    ? audioClip.length - AudioUtil.GetClipPosition(audioClip)
                    : audioClip.length
                ).ToString(@"hh\:mm\:ss\:ff");

                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - width, position.height), property, label, true);

                bool showProgressBar = isPlaying && playAudioClipAttribute.ShowProgressBar;

                if (GUI.Button(new Rect(position.x + position.width - width, position.y, width, position.height / (showProgressBar ? 2 : 1)), playGUIContent))
                    if (isPlaying)
                        AudioUtil.StopClip(audioClip);
                    else
                        AudioUtil.PlayClip(audioClip);

                if (showProgressBar)
                    EditorGUI.ProgressBar(new Rect(position.x + position.width - width, position.y + position.height / 2, width, position.height / 2), AudioUtil.GetClipPosition(audioClip) / audioClip.length, "");

                if (isPlaying)
                    // Forces repaint all the inspector per frame
                    EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
        }
    }
}
using System;
using System.Reflection;
using AdditionalAttributes.AttributeUsage;
using UnityEngine;

namespace AdditionalAttributes
{
    [AttributeUsageAccessibility(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public sealed class ShowIfAttribute : PropertyAttribute
    {
        /// <summary>
        /// Action to take depending of the condition.
        /// </summary>
        public enum ActionMode
        {
            /// <summary>
            /// The property will be hidden or show depending of the condition.
            /// </summary>
            ShowHide,

            /// <summary>
            /// The property will be disabled or enabled depending of the condition.
            /// </summary>
            EnableDisable,
        }

        public string NameOfConditional { get; private set; }
        public bool Goal { get; private set; }
        public ActionMode Mode { get; private set; }

        public bool indented;

        /// <summary>
        /// Action to take depending of the condition.
        /// </summary>
        /// <param name="nameOfConditional">Action to take depending of the condition.</param>
        /// <param name="goal">Required boolean state to show or enable the property.</param>
        public ShowIfAttribute(string nameOfConditional, ActionMode mode = ActionMode.ShowHide, bool goal = true)
        {
            NameOfConditional = nameOfConditional;
            Goal = goal;
            Mode = mode;
        }
    }
}
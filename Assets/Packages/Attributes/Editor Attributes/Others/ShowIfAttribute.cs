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

        public string NameOfConditional { get; }
        public bool Goal { get; }
        public ActionMode Mode { get; }

        public bool indented;

        /// <summary>
        /// Action to take depending of the condition.
        /// </summary>
        /// <param name="nameOfConditional">Action to take depending of the condition.</param>
        /// <param name="goal">Required boolean state to show or enable the property.</param>
        public ShowIfAttribute(string nameOfConditional, ActionMode mode = ActionMode.ShowHide, bool goal = true) : this(nameOfConditional, goal) => Mode = mode;

        /// <summary>
        /// Action to take depending of the condition.
        /// </summary>
        /// <param name="goal">Required boolean state to show or enable the property.</param>
        public ShowIfAttribute(string nameOfConditional, bool goal)
        {
            NameOfConditional = nameOfConditional;
            Goal = goal;
        }
    }
}
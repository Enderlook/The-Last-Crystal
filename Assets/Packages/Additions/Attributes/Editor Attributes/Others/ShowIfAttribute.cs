using Additions.Attributes.AttributeUsage;

using System;
using System.Reflection;

using UnityEngine;

namespace Additions.Attributes
{
    [AttributeUsageAccessibility(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)]
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

        public readonly string nameOfConditional;
        public readonly bool goal;
        public readonly ActionMode mode;

        public bool indented;

        /// <summary>
        /// Action to take depending of the condition.
        /// </summary>
        /// <param name="nameOfConditional">Action to take depending of the condition.</param>
        /// <param name="goal">Required boolean state to show or enable the property.</param>
        public ShowIfAttribute(string nameOfConditional, ActionMode mode = ActionMode.ShowHide, bool goal = true) : this(nameOfConditional, goal) => this.mode = mode;

        /// <summary>
        /// Action to take depending of the condition.
        /// </summary>
        /// <param name="goal">Required boolean state to show or enable the property.</param>
        public ShowIfAttribute(string nameOfConditional, bool goal)
        {
            this.nameOfConditional = nameOfConditional;
            this.goal = goal;
        }
    }
}
﻿using System;
using System.Reflection;
using AdditionalAttributes.AttributeUsage;

namespace AdditionalAttributes.PostCompiling.Internal
{
    [AttributeUsageAccessibility(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)]
    public abstract class BaseExecuteWhenScriptsReloads : Attribute
    {
        /// <summary>
        /// In which loop of the execution will this script executed.<br>
        /// Accept any kind of number.
        /// </summary>
        public int Loop { get; private set; }

        protected BaseExecuteWhenScriptsReloads(int loop = 0) => Loop = loop;
    }
}
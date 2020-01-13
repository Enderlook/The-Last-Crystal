﻿
using System;
using System.Reflection;

namespace Additions.Attributes.PostCompiling.Attributes
{
    /// <summary>
    /// Rules that should be match by the field.
    /// </summary>
    [Flags]
    public enum FieldFlags
    {
        /// <summary>
        /// Non serializable fields by Unity are allowed.<br>
        /// <seealso cref="ReflectionHelper.CanBeSerializedByUnity(FieldInfo)"/>.
        /// </summary>
        NotSerializableByUnity = 1,

        /// <summary>
        /// Serializable fields by Unity are allowed.
        /// <seealso cref="ReflectionHelper.CanBeSerializedByUnity(FieldInfo)"/>.
        /// </summary>
        SerializableByUnity = 1 << 1,

        /// <summary>
        /// Either serializable or not serializable fields by Unity are allowed.<br>
        /// <seealso cref="NotSerializableByUnity"/> and <seealso cref="SerializableByUnity"/>.
        /// </summary>
        EitherSerializableOrNotByUnity = NotSerializableByUnity | SerializableByUnity
    }
}
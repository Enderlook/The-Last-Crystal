﻿using System.Collections.Generic;

using UnityEngine;

namespace Additions.Serializables.Atoms
{
    public abstract class AtomConstant<T> : AtomGet<T>
    {
        [SerializeField]
        protected T value;

        /// <summary>
        /// Value of <see cref="Value"/>.
        /// </summary>
        public override T Value => value;

        private bool Equals(AtomConstant<T> other) => EqualityComparer<T>.Default.Equals(value, other.value);

        public override int GetHashCode() => value.GetHashCode();

        public static bool operator ==(AtomConstant<T> left, AtomConstant<T> right) => left.Equals(right);

        public static bool operator !=(AtomConstant<T> left, AtomConstant<T> right) => left.Equals(right);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            return Equals((AtomConstant<T>)obj);
        }
    }
}
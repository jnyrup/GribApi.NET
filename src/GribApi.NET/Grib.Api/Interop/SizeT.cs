// Copyright 2017 Eric Millin
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Runtime.InteropServices;

namespace Grib.Api.Interop
{
    /// <summary>
    /// SizeT is a variable-size, platform-dependent unsigned integer. 
    /// It can store the maximum size of a theoretically possible object
    /// of any type (including array). Implicitly convertable to UInt.
    /// <para>&#160;</para>
    /// Example:
    /// <code>
    /// <para>&#160;&#160;SizeT size = 1;</para>
    /// <para>&#160;&#160;int[] myArray = new int[size];</para>
    /// <para>&#160;&#160;size = (SizeT) getSomeInt();</para>
    /// <para>&#160;&#160;Int64 big = (Int64) size;</para>
    /// </code>
    /// </summary>
    public struct SizeT
    {
        /// <summary>
        /// The value.
        /// </summary>
        public UIntPtr Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeT"/> struct.
        /// </summary>
        /// <param name="val">The value.</param>
        private SizeT(UIntPtr val) : this()
        {
            Value = val;
        }

#pragma warning disable IDE0051 // Remove unused private members
        /// <summary>
        /// Initializes a new instance of the <see cref="SizeT"/> struct.
        /// </summary>
        /// <param name="val">The value.</param>
        private SizeT(uint val = 0)
            : this((UIntPtr)val)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeT"/> struct.
        /// </summary>
        /// <param name="val">The value.</param>
        private SizeT(int val)
            : this((UIntPtr)val)
        {
        }
#pragma warning restore IDE0051 // Remove unused private members

        /// <summary>
        /// Gets the size of the value's container. 4 on x86, 8 on x64.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public static int Size => Marshal.SizeOf(UIntPtr.Zero);

        /// <summary>
        /// Performs an implicit conversion from <see cref="UIntPtr"/> to <see cref="SizeT"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SizeT(UIntPtr s)
        {
            return new SizeT(s);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="uint"/> to <see cref="SizeT"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SizeT(uint s)
        {
            return new SizeT((UIntPtr)s);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="ulong"/> to <see cref="SizeT"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator SizeT(ulong s)
        {
            return new SizeT((UIntPtr)s);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="int"/> to <see cref="SizeT"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator SizeT(int s)
        {
            return new SizeT((UIntPtr)s);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="long"/> to <see cref="SizeT"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator SizeT(long s)
        {
            return new SizeT((UIntPtr)s);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SizeT"/> to <see cref="UIntPtr"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator UIntPtr(SizeT s)
        {
            return s.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SizeT"/> to <see cref="uint"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator uint(SizeT s)
        {
            return s.Value.ToUInt32();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SizeT"/> to <see cref="ulong"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ulong(SizeT s)
        {
            return s.Value.ToUInt64();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SizeT"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator int(SizeT s)
        {
            return (int)s.Value.ToUInt32();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SizeT"/> to <see cref="long"/>.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator long(SizeT s)
        {
            return (long)s.Value.ToUInt64();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(SizeT a, SizeT b)
        {
            return a.Value == b.Value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(SizeT a, SizeT b)
        {
            return a.Value != b.Value;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            bool isEqual = false;

            if (obj != null && typeof(SizeT).IsAssignableFrom(obj.GetType()))
            {
                isEqual = this == (SizeT)obj;
            }

            return isEqual;
        }
    }
}

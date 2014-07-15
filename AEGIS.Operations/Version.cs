﻿/// <copyright file="Version.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;

namespace ELTE.AEGIS.Operations
{
    /// <summary>
    /// Represents a version number of an operation method.
    /// </summary>
    public class Version : IComparable, IComparable<Version>, IEquatable<Version>
    {
        #region Private constant fields

        /// <summary>
        /// The default version of 1.0.0. This field is read-only.
        /// </summary>
        private const String DefaultVersionString = "1.0.0";

        #endregion

        #region Private fields

        /// <summary>
        /// The major version number.
        /// </summary>
        private readonly Int32 _major;

        /// <summary>
        /// The minor version number.
        /// </summary>
        private readonly Int32 _minor;

        /// <summary>
        /// The revision number.
        /// </summary>
        private readonly Int32 _revision;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the major version number.
        /// </summary>
        /// <value>The major component of the current version number.</value>
        public Int32 Major
        {
            get { return _major; }
        }

        /// <summary>
        /// Gets the minor version number.
        /// </summary>
        /// <value>The minor component of the current version number.</value>
        public Int32 Minor
        {
            get { return _minor; }
        }

        /// <summary>
        /// Gets the revision number.
        /// </summary>
        /// <value>The revision component of the current version number.</value>
        public Int32 Revision
        {
            get { return _revision; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The major version number is less than 0.</exception>
        public Version(Int32 major) : this(major, 0, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The major version number is less than 0.
        /// or
        /// The minor version number is less than 0.
        /// </exception>
        public Version(Int32 major, Int32 minor) : this(minor, 0, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="revision">The revision number.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The major version number is less than 0.
        /// or
        /// The minor version number is less than 0.
        /// or
        /// The revision number is less than 0.
        /// </exception>
        public Version(Int32 major, Int32 minor, Int32 revision)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException("major", "The major version number is less than 0.");
            if (minor < 0)
                throw new ArgumentOutOfRangeException("minor", "The minor version number is less than 0.");
            if (revision < 0)
                throw new ArgumentOutOfRangeException("revision", "The revision number is less than 0.");

            _major = major;
            _minor = minor;
            _revision = revision;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines whether the version is compatible with another.
        /// </summary>
        /// <param name="other">The other version.</param>
        /// <returns><c>true</c> if the two versions are compatible; otherwise <c>false</c>.</returns>
        public Boolean IsCompatible(Version other)
        {
            if (other == null)
                return false;

            return _major == other._major;
        }

        #endregion

        #region IComparable methods

        /// <summary>
        /// Compares the current version with another object.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="System.InvalidOperationException">The object is not the same type as this instance.</exception>
        public Int32 CompareTo(Object obj)
        {
            if (!(obj is Version))
                throw new InvalidOperationException("The object is not the same type as this instance.");

            return CompareTo(obj as Version);
        }

        /// <summary>
        /// Compares the current version with another version.
        /// </summary>
        /// <param name="other">A version to compare with this object.</param>
        /// <returns>A value that indicates the relative order of the versions being compared.</returns>
        public Int32 CompareTo(Version other)
        {
            if (other == null)
                return 1;

            if (_major != other._major)
                return (_major > other._major) ? 1 : -1;
 
            if (_minor != other._minor)
                return (_minor > other._minor) ? 1 : -1;
 
            if (_revision != other._revision)
                return (_revision > other._revision) ? 1 : -1;
 
            return 0;
        }

        #endregion

        #region IEquatable methods

        /// <summary>
        /// Indicates whether the current version is equal to another version.
        /// </summary>
        /// <param name="other">The version to compare with the current version.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public Boolean Equals(Version other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;

            return (_major == other._major) || (_minor == other._minor) || (_revision == other._revision);
        }

        #endregion

        #region Object methods

        /// <summary>
        /// Determines whether the specified object is equal to the current version.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;

            Version version = obj as Version;

            return (_major == version._major) || (_minor == version._minor) || (_revision == version._revision);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Object" />.</returns>
        public override Int32 GetHashCode()
        {
            return (_major << 24 | _minor << 16 | _revision) ^ 961777723;
        }

        /// <summary>
        /// Returns a string that represents the current version.
        /// </summary>
        /// <returns>A string that represents the current version.</returns>
        public override String ToString()
        {
            return String.Concat(_major, ".", _minor, ".", _revision); 
        }

        #endregion

        #region Public static properties

        /// <summary>
        /// Gets the default version.
        /// </summary>
        /// <value>The default version of 1.0.0.</value>
        public static Version Default { get { return Version.Parse(DefaultVersionString); } }

        #endregion

        #region Public static methods

        /// <summary>
        /// Parses the specified version string.
        /// </summary>
        /// <param name="version">The version string.</param>
        /// <returns>The parsed version.</returns>
        /// <exception cref="System.ArgumentNullException">The version is null.</exception>
        /// <exception cref="System.ArgumentException">The version has no components or more than three components.</exception>
        /// <exception cref="System.FormatException">One or more components of the version do not parse into an integer.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">One or more components of the version have a value of less than 0.</exception>
        public static Version Parse(String version)
        {
            if (version == null)
                throw new ArgumentNullException("version", "The version is null.");

            String[] components = version.Split('.');

            if (components.Length < 1 || components.Length > 3)
                throw new ArgumentException("The version has no components or more than three components.", "version");

            Int32 major, minor = 0, revision = 0;
            if (!Int32.TryParse(components[0], out major) ||
                components.Length > 1 && !Int32.TryParse(components[1], out minor) ||
                components.Length > 2 && !Int32.TryParse(components[2], out revision))
            {
                throw new FormatException("One or more components of the version do not parse into an integer.");
            }

            if (major < 0 || minor < 0 || revision < 0)
                throw new ArgumentOutOfRangeException("One or more components of the version have a value of less than 0.");

            return new Version(major, minor, revision);
        }

        /// <summary>
        /// Triues to parse the specified version string.
        /// </summary>
        /// <param name="version">The version string.</param>
        /// <param name="result">The result.</param>
        /// <returns><c>true</c> if the version string was converted successfully; otherwise, <c>false</c>.</returns>
        public static Boolean TryParse(String version, out Version result)
        {
            result = null;

            if (version == null)
                return false;

            String[] components = version.Split('.');

            if (components.Length < 1 || components.Length > 3)
                return false;

            Int32 major, minor = 0, revision = 0;
            if (!Int32.TryParse(components[0], out major) ||
                components.Length > 1 && !Int32.TryParse(components[1], out minor) ||
                components.Length > 2 && !Int32.TryParse(components[2], out revision))
            {
                return false;
            }

            if (major < 0 || minor < 0 || revision < 0)
                return false;

            result = new Version(major, minor, revision);

            return true;
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Version" /> instances are equal.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the instances represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator ==(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;

            return first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the specified <see cref="Version" /> instances are not equal.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the instances do not represent the same value; otherwise, <c>false</c>.</returns>
        public static Boolean operator !=(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return false;

            return !first.Equals(second);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is less than the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is less than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return false;

            return (first.CompareTo(second) < 0);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is greater than the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is greater than the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >(Version first, Version second)
        {
            return (second < first);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is smaller or equal to the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is smaller or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator <=(Version first, Version second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null))
                return true;

            return (first.CompareTo(second) <= 0);
        }

        /// <summary>
        /// Indicates whether the first specified <see cref="Version" /> instance is greater or equal to the second.
        /// </summary>
        /// <param name="first">The first version.</param>
        /// <param name="second">The second version.</param>
        /// <returns><c>true</c> if the first <see cref="Version" /> instance is greater or equal to the second; otherwise, <c>false</c>.</returns>
        public static Boolean operator >=(Version first, Version second)
        {
            return (second <= first);
        }

        #endregion
    }
}

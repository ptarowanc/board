using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MHLab.PATCH
{
    public sealed class Version : IComparable
    {
        private int m_majorReleaseNumber;
        private int m_minorReleaseNumber;
        private int m_maintenanceReleaseNumber;
        private int m_buildNumber;

        public int Major { get { return m_majorReleaseNumber; } }
        public int Minor { get { return m_minorReleaseNumber; } }
        public int Revision { get { return m_maintenanceReleaseNumber; } }
        public int Build { get { return m_buildNumber; } }

        public Version()
        {
            m_majorReleaseNumber = 0;
            m_minorReleaseNumber = 0;
            m_maintenanceReleaseNumber = 0;
            m_buildNumber = 0;
        }

        public Version(string version)
        {
            Version v = null;
            TryParse(version, out v);

            if(v != null)
            {
                m_majorReleaseNumber = v.m_majorReleaseNumber;
                m_minorReleaseNumber = v.m_minorReleaseNumber;
                m_maintenanceReleaseNumber = v.m_maintenanceReleaseNumber;
                m_buildNumber = v.m_buildNumber;
            }
            else
            {
                throw new ArgumentException("Version string isn't valid");
            }
        }

        public static bool TryParse(string input, out Version buildNumber)
        {
            try
            {
                buildNumber = Parse(input);
                return true;
            }
            catch
            {
                buildNumber = null;
                return false;
            }
        }

        public static Version Parse(string buildNumber)
        {
            if (buildNumber == null) throw new ArgumentNullException("buildNumber");

            var versions = buildNumber
                .Split(new[] { '.' },
                       StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .ToList();

            if (versions.Count < 2)
            {
                throw new ArgumentException("BuildNumber string was too short");
            }

            if (versions.Count > 4)
            {
                throw new ArgumentException("BuildNumber string was too long");
            }

            return new Version
            {
                m_majorReleaseNumber = ParseVersion(versions[0]),
                m_minorReleaseNumber = ParseVersion(versions[1]),
                m_maintenanceReleaseNumber = versions.Count > 2 ? ParseVersion(versions[2]) : -1,
                m_buildNumber = versions.Count > 3 ? ParseVersion(versions[3]) : -1
            };
        }

        private static int ParseVersion(string input)
        {
            int version;

            if (!int.TryParse(input, out version))
            {
                throw new FormatException(
                    "Version string was not in a correct format");
            }

            if (version < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "Version",
                    "Versions must be greater than or equal to zero");
            }

            return version;
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}{2}{3}", m_majorReleaseNumber, m_minorReleaseNumber,
                                 m_maintenanceReleaseNumber < 0 ? "" : "." + m_maintenanceReleaseNumber,
                                 m_buildNumber < 0 ? "" : "." + m_buildNumber);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var buildNumber = obj as Version;
            if (buildNumber == null) return 1;
            if (ReferenceEquals(this, buildNumber)) return 0;

            return (m_majorReleaseNumber == buildNumber.m_majorReleaseNumber)
                       ? (m_minorReleaseNumber == buildNumber.m_minorReleaseNumber)
                             ? (m_maintenanceReleaseNumber == buildNumber.m_maintenanceReleaseNumber)
                                   ? m_buildNumber.CompareTo(buildNumber.m_buildNumber)
                                   : m_maintenanceReleaseNumber.CompareTo(buildNumber.m_maintenanceReleaseNumber)
                             : m_minorReleaseNumber.CompareTo(buildNumber.m_minorReleaseNumber)
                       : m_majorReleaseNumber.CompareTo(buildNumber.m_majorReleaseNumber);
        }

        public static bool operator >(Version first, Version second)
        {
            return (first.CompareTo(second) > 0);
        }

        public static bool operator <(Version first, Version second)
        {
            return (first.CompareTo(second) < 0);
        }

        public override bool Equals(object obj)
        {
            return (CompareTo(obj) == 0);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + m_majorReleaseNumber.GetHashCode();
                hash = hash * 23 + m_minorReleaseNumber.GetHashCode();
                hash = hash * 23 + m_maintenanceReleaseNumber.GetHashCode();
                hash = hash * 23 + m_buildNumber.GetHashCode();
                return hash;
            }
        }
    }
}

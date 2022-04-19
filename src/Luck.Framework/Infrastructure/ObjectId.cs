using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Luck.Framework.Infrastructure
{
    public struct ObjectId : IComparable<ObjectId>, IEquatable<ObjectId>
    {
        private static readonly DateTime UnixEpoch;

        private static readonly int StaticMachine;
        private static readonly short StaticPid;
        // high byte will be masked out when generating new ObjectId
        private static int _staticIncrement;
        private static readonly uint[] Lookup32 = Enumerable.Range(0, 256).Select(i =>
        {
            var s = i.ToString("x2");
            return (uint)s[0] + ((uint)s[1] << 16);
        }).ToArray();

        private readonly int _timestamp;

        private readonly int _machine;
        private readonly short _pid;
        private readonly int _increment;

        static ObjectId()
        {
            UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            StaticMachine = GetMachineHash();
            _staticIncrement = new Random().Next();
            StaticPid = (short)GetCurrentProcessId();
        }
        public ObjectId(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(machine),
                    @"The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(increment),
                    @"The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            _timestamp = timestamp;
            _machine = machine;
            _pid = pid;
            _increment = increment;
        }

        public static bool operator ==(ObjectId lhs, ObjectId rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ObjectId lhs, ObjectId rhs)
        {
            return !(lhs == rhs);
        }

        public static ObjectId GenerateNewId()
        {
            return GenerateNewId(GetTimestampFromDateTime(DateTime.UtcNow));
        }

        public static ObjectId GenerateNewId(int timestamp)
        {
            var increment = Interlocked.Increment(ref _staticIncrement) & 0x00ffffff; // only use low order 3 bytes
            return new ObjectId(timestamp, StaticMachine, StaticPid, increment);
        }

        public static string GenerateNewStringId()
        {
            return GenerateNewId().ToString();
        }

        public static byte[] Pack(int timestamp, int machine, short pid, int increment)
        {
            if ((machine & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(machine),
                    @"The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            if ((increment & 0xff000000) != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(increment),
                    @"The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            var bytes = new byte[12];
            bytes[0] = (byte)(timestamp >> 24);
            bytes[1] = (byte)(timestamp >> 16);
            bytes[2] = (byte)(timestamp >> 8);
            bytes[3] = (byte)timestamp;
            bytes[4] = (byte)(machine >> 16);
            bytes[5] = (byte)(machine >> 8);
            bytes[6] = (byte)machine;
            bytes[7] = (byte)(pid >> 8);
            bytes[8] = (byte)pid;
            bytes[9] = (byte)(increment >> 16);
            bytes[10] = (byte)(increment >> 8);
            bytes[11] = (byte)increment;
            return bytes;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetCurrentProcessId()
        {
            return Environment.ProcessId;
        }

        private static int GetMachineHash()
        {
            var hostName = Environment.MachineName; // use instead of Dns.HostName so it will work offline
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(hostName));
            return (hash[0] << 16) + (hash[1] << 8) + hash[2]; // use first 3 bytes of hash
        }

        private static int GetTimestampFromDateTime(DateTime timestamp)
        {
            return (int)Math.Floor((ToUniversalTime(timestamp) - UnixEpoch).TotalSeconds);
        }


        public int CompareTo(ObjectId other)
        {
            var r = _timestamp.CompareTo(other._timestamp);
            if (r != 0)
            {
                return r;
            }

            r = _machine.CompareTo(other._machine);
            if (r != 0)
            {
                return r;
            }

            r = _pid.CompareTo(other._pid);
            if (r != 0)
            {
                return r;
            }

            return _increment.CompareTo(other._increment);
        }

        public bool Equals(ObjectId rhs)
        {
            return
                _timestamp == rhs._timestamp &&
                _machine == rhs._machine &&
                _pid == rhs._pid &&
                _increment == rhs._increment;
        }

#pragma warning disable CS8765 // 参数类型的为 Null 性与重写成员不匹配(可能是由于为 Null 性特性)。
        public override bool Equals(object obj)
#pragma warning restore CS8765 // 参数类型的为 Null 性与重写成员不匹配(可能是由于为 Null 性特性)。
        {
            if (obj is ObjectId id)
            {
                return Equals(id);
            }

            return false;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = 37 * hash + _timestamp.GetHashCode();
            hash = 37 * hash + _machine.GetHashCode();
            hash = 37 * hash + _pid.GetHashCode();
            hash = 37 * hash + _increment.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Converts the ObjectId to a byte array.
        /// </summary>
        /// <returns>A byte array.</returns>
        public byte[] ToByteArray()
        {
            return Pack(_timestamp, _machine, _pid, _increment);
        }

        /// <summary>
        /// Returns a string representation of the value.
        /// </summary>
        /// <returns>A string representation of the value.</returns>
        public override string ToString()
        {
            return ToHexString(ToByteArray());
        }

        /// <summary>
        /// Converts a byte array to a hex string.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>A hex string.</returns>
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var result = new char[bytes.Length * 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var val = Lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }

            return new string(result);
        }

        /// <summary>
        /// Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }

            if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }

            return dateTime.ToUniversalTime();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Contoso.Aids
{
    public static class GetRandom
    {
        public static sbyte Int8(sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue)
            => Convert.ToSByte(rnd(min, max));
        public static short Int16(short min = short.MinValue, short max = short.MaxValue)
            => Convert.ToInt16(rnd(min, max));
        public static int Int32(int min = int.MinValue, int max = int.MaxValue)
            => Convert.ToInt32(rnd(min, max));
        public static long Int64(long min = long.MinValue, long max = long.MaxValue)
            => Convert.ToInt64(rnd(min, max));
        public static byte UInt8(byte min = byte.MinValue, byte max = byte.MaxValue)
            => Convert.ToByte(rnd(min, max));
        public static ushort UInt16(ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
            => Convert.ToUInt16(rnd(min, max));
        public static uint UInt32(uint min = uint.MinValue, uint max = uint.MaxValue)
            => Convert.ToUInt32(rnd(min, max));
        public static ulong UInt64(ulong min = ulong.MinValue, ulong max = ulong.MaxValue)
            => Convert.ToUInt64(rnd(min, max));
        public static float Float(float min = float.MinValue, float max = float.MaxValue)
            => Convert.ToSingle(rnd(min, max));
        public static double Double(double min = double.MinValue, double max = double.MaxValue)
              => rnd(min, max);
        public static decimal Decimal(decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
            => Convert.ToDecimal(rnd(Convert.ToDouble(min), Convert.ToDouble(max)));
        public static DateTime DateTime(DateTime? minValue = null, DateTime? maxValue = null)
        {
            DateTime min = minValue ?? System.DateTime.MinValue;
            DateTime max = maxValue ?? System.DateTime.MaxValue;
            var ticks = Convert.ToInt64(rnd(Convert.ToDouble(min.Ticks), Convert.ToDouble(max.Ticks)));
            return new DateTime(ticks);
        }
        public static string String(uint minLength = 5, uint maxLength = 30)
        {
            var min = getMin(minLength, maxLength);
            var max = getMax(minLength, maxLength);
            var rnd = new Random();
            var length = Int32((int)min, (int)max);
            byte[] bytes = new byte[length];
            rnd.NextBytes(bytes);
            return Encoding.UTF8.GetString(bytes);
        }
        public static string Text(uint minLength = 5, uint maxLength = 30)
        {
            var b = new StringBuilder();
            var size = UInt32(minLength, maxLength);
            for (var i = 0; i < size; i++) b.Append(Char('a', 'z'));
            return b.ToString();
        }
        internal static T getMax<T>(T x, T y) where T : IComparable => x.CompareTo(y) > 0 ? x : y;
        internal static T getMin<T>(T x, T y) where T : IComparable => x.CompareTo(y) < 0 ? x : y;
        internal static double rnd(double minValue, double maxValue)
        {
            var rnd = new Random().NextDouble();
            var min = getMin(minValue, maxValue) / 10.0;
            var max = getMax(minValue, maxValue) / 10.0;
            return (rnd * (max - min) + min) * 10.0;
        }
        public static bool Bool() => Int32() % 2 == 0;
        public static char Char(char min = char.MinValue, char max = char.MaxValue)
            => (char)UInt16(min, max);
        public static Color Color()
            => System.Drawing.Color.FromArgb(UInt8(), UInt8(), UInt8());
        public static T EnumOf<T>() => (T)EnumOf(typeof(T));
        public static object EnumOf(Type t)
        {
            var count = GetEnum.Count(t);
            var index = Int32(0, count);
            return GetEnum.ValueByIndex(t, index);
        }
        public static TimeSpan TimeSpan() => new TimeSpan(Int64());
        public static object ValueOf<T>() => ValueOf(typeof(T));
        public static object ValueOf(Type t)
        {
            var x = Nullable.GetUnderlyingType(t);
            if (!(x is null)) t = x;
            if (t.IsArray) return ArrayOf(t.GetElementType());
            if (t.IsEnum) return EnumOf(t);
            if (t == typeof(string)) return String();
            if (t == typeof(char)) return Char();
            if (t == typeof(Color)) return Color();
            if (t == typeof(bool)) return Bool();
            if (t == typeof(DateTime)) return DateTime();
            if (t == typeof(decimal)) return Decimal();
            if (t == typeof(double)) return Double();
            if (t == typeof(float)) return Float();
            if (t == typeof(byte)) return UInt8();
            if (t == typeof(ushort)) return UInt16();
            if (t == typeof(uint)) return UInt32();
            if (t == typeof(ulong)) return UInt64();
            if (t == typeof(sbyte)) return Int8();
            if (t == typeof(short)) return Int16();
            if (t == typeof(int)) return Int32();
            if (t == typeof(long)) return Int64();
            if (t == typeof(TimeSpan)) return TimeSpan();
            if (t == typeof(char?)) return Char();
            if (t == typeof(Color?)) return Color();
            if (t == typeof(bool?)) return Bool();
            if (t == typeof(DateTime?)) return DateTime();
            if (t == typeof(decimal?)) return Decimal();
            if (t == typeof(double?)) return Double();
            if (t == typeof(float?)) return Float();
            if (t == typeof(byte?)) return UInt8();
            if (t == typeof(ushort?)) return UInt16();
            if (t == typeof(uint?)) return UInt32();
            if (t == typeof(ulong?)) return UInt64();
            if (t == typeof(sbyte?)) return Int8();
            if (t == typeof(short?)) return Int16();
            if (t == typeof(int?)) return Int32();
            if (t == typeof(long?)) return Int64();
            if (t == typeof(TimeSpan)) return TimeSpan();
            return ObjectOf(t);
        }
        public static object ArrayOf(Type t)
        {
            if (t is null) return null;
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(t);
            var list = (IList)Activator.CreateInstance(constructedListType);
            for (var i = 0; i < UInt8(3, 10); i++) list.Add(ValueOf(t));
            var array = Array.CreateInstance(t, list.Count);
            list.CopyTo(array, 0);
            return array;
        }

        public static T ObjectOf<T>() => (T)ObjectOf(typeof(T));
        public static object ObjectOf(Type t)
        {
            var o = CreateNew.Instance(t);
            SetRandom.Values(o);
            return o;
        }
        public static string Email()
            => $"{Text()}.{Text()}@{Text()}.{Text()}";
        public static string Password()
            => $"{Text()}{Char('\x20', '\x2f')}{UInt32()}.{Text().ToUpper()}";
        public static List<T> List<T>(Func<T> func)
        {
            var list = new List<T>();
            for (var i = 0; i < UInt8(2, 10); i++) list.Add(func());
            return list;
        }
        public static object AnyDouble(byte minValue = 0, byte maxValue = 100)
        {
            var i = UInt8();
            return (i % 10) switch
            {
                0 => Int32(minValue, maxValue),
                1 => UInt32(minValue, maxValue),
                2 => Float(minValue, maxValue),
                3 => Int8(0),
                4 => UInt8(minValue, maxValue),
                5 => Int16(minValue, maxValue),
                6 => UInt16(minValue, maxValue),
                7 => Int64(minValue, maxValue),
                8 => UInt64(minValue, maxValue),
                _ => Double(minValue, maxValue)
            };
        }
        public static object AnyInt(byte minValue = 0, byte maxValue = 100)
        {
            var i = UInt8();
            return (i % 5) switch
            {
                0 => Int8(0),
                1 => UInt8(minValue, maxValue),
                2 => Int16(minValue, maxValue),
                4 => UInt16(minValue, maxValue),
                _ => Int32(minValue, maxValue)
            };
        }
        public static object AnyValue()
        {
            var i = Int32();
            return (i % 10) switch
            {
                0 => (object)DateTime(),
                1 => String(),
                2 => Char(),
                3 => Int32(),
                4 => Double(),
                5 => Decimal(),
                6 => UInt32(),
                7 => Float(),
                8 => Int8(),
                9 => UInt8(),
                10 => Int16(),
                11 => UInt16(),
                12 => Int64(),
                13 => UInt64(),
                _ => String()
            };
        }
    }
}

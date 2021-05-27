using System.Linq;
using System.Collections.Generic;

using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System;
using System.Text;

namespace Finix.CsUtils
{

#if !NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// Emulates System.Text.Rune
    ///
    /// Represents a Unicode scalar value ([ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive).
    /// </summary>
    /// <remarks>
    /// This type's constructors and conversion operators validate the input, so consumers can call the APIs
    /// assuming that the underlying <see cref="Rune"/> instance is well-formed.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public readonly struct Rune : IComparable, IComparable<Rune>, IEquatable<Rune>
    {
        public int Value { get; }

        public Rune(int codePoint)
        {
            Value = codePoint;
        }

        public Rune(char c)
        {
            Value = (int) c;
        }

        public int CompareTo(Rune other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(Rune other)
        {
            return Value.Equals(other.Value);
        }

        public int CompareTo(object? obj)
        {
            return Value.CompareTo(obj);
        }

        public override bool Equals(object obj)
        {
            return obj is Rune r && Equals(r);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return ((char) this).ToString();
        }

        public static explicit operator Rune(char c)
        {
            return new Rune(c);
        }

        public static explicit operator Rune(int c)
        {
            return new Rune(c);
        }

        public static explicit operator char(Rune r)
        {
            return (char) r.Value;
        }

        public static explicit operator int(Rune r)
        {
            return (int) r.Value;
        }

        public static bool operator ==(Rune lhs, Rune rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Rune lhs, Rune rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator <(Rune left, Rune right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Rune left, Rune right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Rune left, Rune right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Rune left, Rune right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool IsValid(int codePoint)
        {
            return codePoint > 0;
        }

        public static bool IsControl(Rune r)
        {
            return Char.IsControl((char) r.Value);
        }

        public static bool IsSymbol(Rune r)
        {
            return Char.IsSymbol((char) r.Value);
        }

        public static bool IsPunctuation(Rune r)
        {
            return Char.IsPunctuation((char) r.Value);
        }

        public static bool IsWhiteSpace(Rune r)
        {
            return Char.IsWhiteSpace((char) r.Value);
        }

        public static bool IsLetter(Rune r)
        {
            return Char.IsLetter((char) r.Value);
        }

        public static bool IsLetterOrDigit(Rune r)
        {
            return Char.IsLetterOrDigit((char) r.Value);
        }

        public static bool IsUpper(Rune r)
        {
            return Char.IsUpper((char) r.Value);
        }

        public static bool IsLower(Rune r)
        {
            return Char.IsLower((char) r.Value);
        }
    }

    public static class RuneEnumeratorExtensions
    {
        public static IEnumerable<Rune> EnumerateRunes(this string str)
        {
            foreach (var c in str.ToCharArray())
            {
                yield return new Rune(c);
            }
        }
    }
#endif
}

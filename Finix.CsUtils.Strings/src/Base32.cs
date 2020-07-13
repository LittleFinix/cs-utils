using System.Runtime.InteropServices;
using System.Linq;
using System.Buffers;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Finix.CsUtils
{
    public static class Base32
    {
        public const int ByteBitCount = 8;

        public const int BitWidth = 5;

        public const int LCM = BitWidth * ByteBitCount;

        public const int MinEncodedBytes = LCM / ByteBitCount;

        private static readonly byte[] Digits;
        private static readonly byte BitMask;

        // private static readonly int Shift;

        private static Dictionary<byte, byte> CharacterMap = new Dictionary<byte, byte>();

        public const byte Padding = (byte) '=';

        // private const string SEPARATOR = "-";

        static Base32()
        {
            Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray().Select(c => (byte) c).ToArray();
            BitMask = (byte) (Digits.Length - 1);
            // Shift = numberOfTrailingZeros(Digits.Length);

            for (int i = 0; i < Digits.Length; i++)
                CharacterMap[Digits[i]] = (byte) i;
        }

        public static int GetMaxEncodedToUtf8Length(int length)
        {
            return length * ByteBitCount / BitWidth + (length % LCM);
        }

        public static int GetMaxDecodedFromUtf8Length(int length)
        {
            return length * BitWidth / ByteBitCount;
        }

        public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
        {
            if (isFinalBlock || utf8.BinarySearch(Padding) < 0)
                return DecodeBlock(utf8, bytes, out bytesConsumed, out bytesWritten);

            var stat = DecodeBlock(utf8.Slice(0, utf8.Length - utf8.Length % MinEncodedBytes), bytes, out bytesConsumed, out bytesWritten);

            if (stat != OperationStatus.Done)
                return stat;

            return utf8.Length % MinEncodedBytes != 0 ? OperationStatus.NeedMoreData : OperationStatus.Done;
        }

        public static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, out int bytesWritten)
        {
            throw new NotSupportedException();
        }

        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
        {
            if (isFinalBlock)
                return EncodeBlock(bytes, utf8, out bytesConsumed, out bytesWritten);

            var stat = EncodeBlock(bytes.Slice(0, bytes.Length - bytes.Length % MinEncodedBytes), utf8, out bytesConsumed, out bytesWritten, false);

            if (stat != OperationStatus.Done)
                return stat;

            return bytes.Length % MinEncodedBytes != 0 ? OperationStatus.NeedMoreData : OperationStatus.Done;
        }

        public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer, int dataLength, out int bytesWritten)
        {
            throw new NotSupportedException();
        }

        private static void EncodeSlice(ulong val, Span<byte> encoded)
        {
            for (int i = 7; i >= 0; i--)
            {
                encoded[i] = Digits[val & BitMask];
                val >>= 5;
            }
        }

        private static void DecodeSlice(ReadOnlySpan<byte> encoded, out ulong val)
        {
            val = 0;
            for (int i = 0; i < 8; i++)
            {
                val <<= 5;
                val |= (uint) (encoded[i] == 0 ? 0 : CharacterMap[encoded[i]] & BitMask);
            }
        }

        private static (int l, int h, int il, int ih) GetFiveBitSlice(int index)
        {
            var i = (index / 8) * 5;
            var h = 8;
            var l = 8;
            var il = i;
            var ih = i;

            switch (index % 8)
            {
                case 0:
                    l = 3;
                    break;

                case 1:
                    l = -2;
                    h = 6;
                    ih += 1;
                    break;

                case 2:
                    l = 1;
                    il += 1;
                    break;

                case 3:
                    l = -4;
                    h = 4;
                    il += 1;
                    ih += 2;
                    break;

                case 4:
                    l = -1;
                    h = 7;
                    il += 2;
                    ih += 3;
                    break;

                case 5:
                    l = 2;
                    il += 3;
                    break;

                case 6:

                    l = -3;
                    h = 5;
                    il += 3;
                    ih += 4;
                    break;

                case 7:
                    l = 0;
                    il += 4;
                    break;
            }

            // var n = (l & BitMask) | (h & BitMask);
            // return n;
            return (l, h, il, ih);
        }

        private static int GetFiveBitBlock(ReadOnlySpan<byte> bytes, int index)
        {
            int b = (index * 8) % 40;
            int ls = (index * 8) % 5;
            int rs = 8 - ls;

            var i = (index / 8) * 5;
            var h = 0;
            var l = 0;

            switch (index % 8)
            {
                case 0:
                    l = bytes[i] >> 3;
                    break;

                case 1:
                    l = bytes[i] << 2;
                    h = bytes[i + 1] >> 6;
                    break;

                case 2:
                    l = bytes[i + 1] >> 1;
                    break;

                case 3:
                    l = bytes[i + 1] << 4;
                    h = bytes[i + 2] >> 4;
                    break;

                case 4:
                    l = bytes[i + 2] << 1;
                    h = bytes[i + 3] >> 7;
                    break;

                case 5:
                    l = bytes[i + 3] >> 2;
                    break;

                case 6:
                    l = bytes[i + 3] << 3;
                    h = bytes[i + 4] >> 5;
                    break;

                case 7:
                    l = bytes[i + 4];
                    break;
            }

            // var (l, h, il, ih) = GetFiveBitSlice(index);

            // var n = ( (bytes[il] >> l) & BitMask )
            //     | ( (bytes[ih] >> h) & BitMask );

            var n = (l & BitMask) | (h & BitMask);
            return n;
        }

        private static void SetFiveBitBlock(Span<byte> bytes, int index, int val)
        {

        }

        private static OperationStatus EncodeBlock(ReadOnlySpan<byte> bytes, Span<byte> str, out int read, out int written, bool padding = true)
        {
            written = 0;
            read = 0;
            var blocks = bytes.Length - (bytes.Length % 5);

            if (str.Length < GetMaxEncodedToUtf8Length(blocks))
                return OperationStatus.DestinationTooSmall;

            for (read = 0; read < blocks; read += 5)
            {
                ulong top = 0;

                for (int i = 0; i < 5; i++)
                {
                    top |= (ulong) bytes[read + i] << (8 * (4 - i));
                }

                EncodeSlice(top, str.Slice(written));
                written += 8;
            }

            var left = bytes.Length - read;

            if (padding && left > 0)
            {
                var buff = new byte[5];

                bytes.Slice(read).CopyTo(buff);

                ulong top = 0;

                for (int i = 0; i < 5; i++)
                {
                    top |= (ulong) buff[i] << (8 * (4 - i));
                }

                EncodeSlice(top, str.Slice(written));

                var wlen = left * ByteBitCount / BitWidth + 1;
                var padlen = (8 - wlen) % 8;

                str.Slice(written + wlen, padlen).Fill(Padding);
                written += 8;
                read += left;
            }

            return left > 0 ? OperationStatus.NeedMoreData : OperationStatus.Done;
        }

        private static OperationStatus DecodeBlock(ReadOnlySpan<byte> str, Span<byte> bytes, out int read, out int written)
        {
            str = str.TrimEnd(Padding);

            read = 0;
            written = 0;
            var blocks = str.Length - (str.Length % 8);

            if (bytes.Length < GetMaxDecodedFromUtf8Length(str.Length))
                return OperationStatus.DestinationTooSmall;

            for (read = 0; read < blocks; read += 8)
            {
                DecodeSlice(str.Slice(read), out var top); // , out var bottom

                for (var i = 0; i < 5; i++)
                    bytes[written + i] = (byte) (top >> (8 * (4 - i)));

                written += 5;
            }

            var left = str.Length - read;

            if (left > 0)
            {
                var buff = new byte[8];
                str.Slice(read).CopyTo(buff);

                DecodeSlice(buff, out var top);

                var count = left * BitWidth / ByteBitCount;

                for (int i = 0; i < count; i++)
                    bytes[written++] = (byte) (top >> 8 * (4 - i));

                read += left;
            }

            return OperationStatus.Done;
        }
    }
}

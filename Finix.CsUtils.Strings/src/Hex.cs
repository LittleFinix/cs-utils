using System.Globalization;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Buffers;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Finix.CsUtils
{
    public static class Hex
    {
        public static int GetMaxEncodedToUtf8Length(int length)
        {
            return length * 3;
        }

        public static int GetMaxDecodedFromUtf8Length(int length)
        {
            return length;
        }

        public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
        {
            bytesConsumed = 0;
            bytesWritten = 0;

            Rune? hi = null;
            Rune? lo = null;

            var result = OperationStatus.Done;
            while (bytesConsumed < utf8.Length
                && (result = Rune.DecodeFromUtf8(utf8.Slice(bytesConsumed), out var rune, out var consumed)) == OperationStatus.Done)
            {
                bytesConsumed += consumed;

                if (!Rune.IsLetterOrDigit(rune) || hi is object)
                {
                    if (hi is null)
                        continue;

                    if (Rune.IsLetterOrDigit(rune))
                        lo = rune;

                    bytes[bytesWritten++] = byte.Parse("" + hi + (lo ?? new Rune(0)), NumberStyles.HexNumber | NumberStyles.AllowHexSpecifier);

                    hi = null;
                    lo = null;
                }
                else
                {
                    hi = rune;
                    result = OperationStatus.NeedMoreData;
                }
            }

            return result;
        }

        public static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, out int bytesWritten)
        {
            throw new NotSupportedException();
        }

        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
        {
            bytesWritten = 0;
            for (bytesConsumed = 0; bytesConsumed < bytes.Length; bytesConsumed++)
            {
                var str = String.Format(" {0:X2}", bytes[bytesConsumed]);
                // Console.WriteLine(str);

                var strBytes = Encoding.UTF8.GetBytes(str);
                strBytes.CopyTo(utf8.Slice(bytesWritten));
                bytesWritten += strBytes.Length;
            }

            return OperationStatus.Done;
        }

        public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer, int dataLength, out int bytesWritten)
        {
            throw new NotSupportedException();
        }
    }
}

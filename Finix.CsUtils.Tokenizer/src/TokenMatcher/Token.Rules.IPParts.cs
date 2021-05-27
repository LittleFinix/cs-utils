using System.Text;
using System.Buffers;
using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Finix.CsUtils
{
    public abstract partial class Token : ICloneable
    {
        public static class IPParts
        {
            public static readonly Token IPv4Separator = T('.');

            public static readonly Token IPv6Separator = T(':');

            public static readonly Token IPv6CollapsedSeparator = T("::");

            public static readonly Token DecOctet = (
                R(
                    DIGIT /
                    (T('1', '9') + DIGIT) /
                    (T('1') + DIGIT + DIGIT) /
                    (T('2') + T('0', '4') + DIGIT) /
                    (T("25") + T('0', '5')),
                    m => new[] { Byte.Parse(m.AsString()) }
                )
            )
                .Literal()
                .Named("dec-octet");

            public static readonly Token H16 = (
                R(
                    (1, 4) * HEXDIG,
                    m => BitConverter.GetBytes(UInt16.Parse(m.AsString(), NumberStyles.HexNumber))
                )
            )
                .Literal()
                .Named("h16");

            public static readonly Token IPv4Address = (
                DecOctet + (3, 3) * ('.' + DecOctet)
            )
                .Named("IPv4address");

            public static readonly Token LS32 = (
                (H16 + IPv6Separator + H16) / IPv4Address
            )
                .Named("ls32");

            public static readonly Token IPv6address = (
                ((6, 6) * (H16 + IPv6Separator) + LS32)
                / (IPv6CollapsedSeparator + (5, 5) * (H16 + IPv6Separator) + LS32)
                / (H16 + IPv6CollapsedSeparator + (4, 4) * (H16 + IPv6Separator) + LS32)
                / ((0, 1) * (H16 + IPv6Separator) + H16 + IPv6CollapsedSeparator + (3, 3) * (H16 + IPv6Separator) + LS32)
                / ((0, 2) * (H16 + IPv6Separator) + H16 + IPv6CollapsedSeparator + (2, 2) * (H16 + IPv6Separator) + LS32)
                / ((0, 3) * (H16 + IPv6Separator) + H16 + IPv6CollapsedSeparator + H16 + IPv6Separator + LS32)
                / ((0, 4) * (H16 + IPv6Separator) + H16 + IPv6CollapsedSeparator + LS32)
                / ((0, 5) * (H16 + IPv6Separator) + H16 + IPv6CollapsedSeparator + H16)
                / ((0, 6) * (H16 + IPv6Separator) + H16 + IPv6CollapsedSeparator)
            )
                .Named("IPv6address");

            public static readonly Token IPvFutureVersion = (
                I("v") + (1 * HEXDIG)
            )
                .Combined()
                .Named("IPvFutureVersion");

            public static readonly Token IPvFutureString = (
                1 * (
                    ALPHA / DIGIT / '-' / '.' / '_'
                    / '~' / ':' / '!' / '$' / '&'
                    / '\'' / '(' / ')' / '*' / '+'
                    / ',' / ';' / '='
                )
            )
                .Combined()
                .Named("IPvFutureString");

            public static readonly Token IPvFuture = (
                IPvFutureVersion + '.' + IPvFutureString
            )
                .Combined()
                .Named("IPvFuture");
        }
    }
}

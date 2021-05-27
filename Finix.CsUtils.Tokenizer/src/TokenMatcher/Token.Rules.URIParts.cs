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
        public static class URIParts
        {
            public static readonly Token Unreserved = (
                ALPHA / DIGIT / '-' / '.' / '_' / '~'
            )
                .Named("unreserved");

            public static readonly Token GenDelims = (
                T(':') / '/' / '?' / '#' / '[' / ']' / '@'
            )
                .Named("gen-delims");

            public static readonly Token SubDelims = (
                T('!') / '$' / '&' / '\'' / '(' / ')'
                      / '*' / '+' / ',' / ';' / '='
            )
                .Named("sub-delims");

            public static readonly Token Reserved = (
                GenDelims + SubDelims
            )
                .Named("reserved");

            public static readonly Token PctEncoded = (
                !T('%') + R(HEXDIG + HEXDIG, value => new[] { byte.Parse(value.AsString(), NumberStyles.HexNumber) })
            )
                .Named("pct-encoded");

            public static readonly Token IPLiteral = (
                '[' + IPParts.IPv6address / IPParts.IPvFuture + ']'
            )
                .Combined()
                .Named("ip-literal");

            public static readonly Token RegName = (
                0 * (Unreserved / PctEncoded / SubDelims)
            )
                .Combined()
                .Named("reg-name");

            public static readonly Token Host = (
                IPLiteral / IPParts.IPv4Address / RegName
            )
                .Combined()
                .Named("host");

            public static readonly Token Port = (
                0 * DIGIT
            )
                .Combined()
                .Named("port");

            public static readonly Token Userinfo = (
                0 * (Unreserved / PctEncoded / SubDelims / ':')
            )
                .Combined()
                .Named("userinfo");

            public static readonly Token Scheme = (
                ALPHA + 0 * (Unreserved - '~')
            )
                .Combined()
                .Named("scheme");

            public static readonly Token Authority = (
                0 * (Userinfo + '@') + Host + 0 * (':' + Port)
            )
                .Combined()
                .Named("authority");

            public static readonly Token PChar = (
                Unreserved / PctEncoded / SubDelims / ':' / '@'
            )
                .Named("pchar");

            public static readonly Token Segment = (
                0 * PChar
            )
                .Combined()
                .Named("segment");

            public static readonly Token SegmentNZ = (
                1 * PChar
            )
                .Combined()
                .Named("segment-nz");

            public static readonly Token SegmentNZNC = (
                1 * (PChar - ':')
            )
                .Combined()
                .Named("segment-nz-nc");

            public static readonly Token PathAbEmpty = (
                0 * ('/' + Segment)
            )
                .Combined()
                .Named("path-abempty");

            public static readonly Token PathAbsolute = (
                '/' + (0, 1) * (SegmentNZ + 0 * ('/' + Segment))
            )
                .Combined()
                .Named("path-absolute");

            public static readonly Token PathNoScheme = (
                SegmentNZNC + 0 * ('/' + Segment)
            )
                .Combined()
                .Named("path-noscheme");

            public static readonly Token PathRootless = (
                SegmentNZ + 0 * ('/' + Segment)
            )
                .Combined()
                .Named("path-rootless");

            public static readonly Token Path = (
                PathAbEmpty
                / PathAbsolute
                / PathNoScheme
                / PathRootless
                / ((0, 0) * PChar)
            )
                .Combined()
                .Named("path");

            public static readonly Token HierPart = (
                ("//" + Authority + PathAbEmpty)
                / PathAbsolute
                / PathRootless
                / ((0, 0) * PChar)
            )
                .Combined()
                .Named("hier-part");

            public static readonly Token Query = (
                0 * (PChar / '/' / '?')
            )
                .Combined()
                .Named("query");

            public static readonly Token Fragment = (
                0 * (PChar / '/' / '?')
            )
                .Combined()
                .Named("fragment");
        }
    }
}

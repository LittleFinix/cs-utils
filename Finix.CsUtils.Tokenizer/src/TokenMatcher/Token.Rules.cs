using System.Text;
using System.Buffers;
using System.IO;
using System.Numerics;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Finix.CsUtils
{
    public abstract partial class Token : ICloneable
    {
        public static readonly Token ALPHA = (T(0x41, 0x5A) / T(0x61, 0x7A)).Named("ALPHA");

        public static readonly Token CHAR = T(0, 0x7F).Named("CHAR");

        public static readonly Token CR = T('\r').Named("CR");

        public static readonly Token LF = T('\n').Named("LF");

        public static readonly Token CRLF = (CR + LF).Named("CRLF");

        public static readonly Token LFCR = (LF + CR).Named("LFCR");

        public static readonly Token EOL = (CRLF / LFCR / CR / LF).Named("EOL");

        public static readonly Token CTL = (T(0x00, 0x1F) / 0x7F).Named("CTL");

        public static readonly Token DIGIT = T(0x30, 0x39).Named("DIGIT");

        public static readonly Token DQUOTE = T('"').Named("DQUOTE");

        public static readonly Token HEXDIG = (DIGIT / T('a', 'f') / T('A', 'F')).Named("HEXDIG");

        public static readonly Token HTAB = T(0x09).Named("HTAB");

        public static readonly Token SP = T(0x20).Named("SP");

        public static readonly Token WSP = (SP / HTAB).Named("WSP");

        public static readonly Token LWSP = (0 * (WSP / (CRLF + WSP))).Named("LWSP");

        public static readonly Token AWSP = (WSP / EOL).Named("AWSP");

        public static readonly Token VCHAR = T(0x21, 0x7E).Named("VCHAR");

        public static readonly Token OCTET = T(0x00, 0xFF).Named("OCTET");

        public static readonly Token UTF8_Tail = T(0x80, 0xBF).Named("UTF8-tail");

        public static readonly Token UTF8_1 = T(0, 0x7F).Named("UTF8-1");

        public static readonly Token UTF8_2 = (
            T(0xC2, 0xDF) + UTF8_Tail
        ).Named("UTF8-2");

        public static readonly Token UTF8_3 = (
            (T(0xE0) + T(0xA0, 0xBF) + UTF8_Tail) /
            (T(0xE1, 0xEC) + (2, 2) * UTF8_Tail) /
            (T(0xED) + T(0x80, 0x9F) + UTF8_Tail) /
            (T(0xEE, 0xEF) + (2, 2) * UTF8_Tail)
        ).Named("UTF8-3");

        public static readonly Token UTF8_4 = (
            (T(0xF0) + T(0x90, 0xBF) + (2, 2) * UTF8_Tail) /
            (T(0xF1, 0xF3) + (3, 3) * UTF8_Tail) /
            (T(0xF4) + T(0x80, 0x8F) + (2, 2) * UTF8_Tail)
        ).Named("UTF8-4");

        public static readonly Token UTF8 = (
            UTF8_1 / UTF8_2 / UTF8_3 / UTF8_4
        ).Named("UTF8-char");

        public static readonly Token UTF8_Octets = (0 * UTF8).Named("UTF8-octets");

        public static readonly Token IP = (
            IPParts.IPv4Address / IPParts.IPv6address / IPParts.IPvFuture
        )
            .Combined()
            .Named("IP");

        public static readonly Token URI = (
            URIParts.Scheme + T(':') + URIParts.HierPart +
            (0, 1) * URIParts.Query +
            (0, 1) * URIParts.Fragment
        )
            .Combined()
            .Named("URI");
    }
}

using System.Linq;
using System.Buffers.Text;
using System.Text;
using System.Collections.Generic;
using System.Text.Unicode;

namespace Finix.CsUtils
{
    /// <summary>
    /// Contains a list of UTF8 Runes categorized by type.
    /// </summary>
    public static class Runes
    {
        // ! Please keep these ordered by ascending code-point, per category.

        public static IReadOnlySet<Rune> BreakingWhiteSpace { get; } = new HashSet<Rune>() {
            new Rune(0x000A), // LF
            new Rune(0x000B), // Vertical Tab
            new Rune(0x000C), // Form Feed
            new Rune(0x000D), // CR
            new Rune(0x0085), // Next Line
            new Rune(0x2028), // Line Separator
            new Rune(0x2029), // Paragraph Separator
        };

        public static IReadOnlySet<Rune> BreakableWhiteSpace { get; } = new HashSet<Rune>(BreakingWhiteSpace) {
            new Rune(0x0009), // Tab
            new Rune(0x0020), // Space
            new Rune(0x180E), // Mongolian Vowel Separator
            new Rune(0x180E), // Mongolian Vowel Separator
            new Rune(0x2000), // EN Quad
            new Rune(0x2001), // EM Quad
            new Rune(0x2002), // EN Space
            new Rune(0x2003), // EM Space
            new Rune(0x2004), // 1/3 EM Space
            new Rune(0x2005), // 1/4 EM Space
            new Rune(0x2006), // 1/6 EM Space
            new Rune(0x2008), // Punctuation Space
            new Rune(0x2009), // Thin Space
            new Rune(0x200A), // Hair Space
            new Rune(0x205F), // Medium Mathematical Space
            new Rune(0x3000), // Ideographic Space
        };

        public static IReadOnlySet<Rune> NonBreakableWhiteSpace { get; } = new HashSet<Rune>() {
            new Rune(0x1680), // NBSP
            new Rune(0x2007), // Figure Space
            new Rune(0x202F), // Narrow NBSP
        };

        public static IReadOnlySet<Rune> Whitespace { get; } = new HashSet<Rune>(BreakableWhiteSpace.Concat(NonBreakableWhiteSpace));
    }
}

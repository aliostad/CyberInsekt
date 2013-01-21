using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CyberInsekt.LinkExtraction
{
    internal class AnchorRegex
    {

        const string AnchorPattern = "<a\\s+.*?\\s+href\\s*=\\s*[\"']?([^\\s\"'>]+)[\"']?";

        private static readonly Regex _extractor = new Regex(AnchorPattern);
        public static IEnumerable<string> FindHrefs(string content)
        {
            var matches = _extractor.Matches(content);
            return matches.Cast<Match>()
                .Select(x => x.Groups[1].Value);
        }
    }
}

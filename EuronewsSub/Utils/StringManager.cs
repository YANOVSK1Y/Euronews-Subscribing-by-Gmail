using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EuronewsSub.Utils
{
    public static class StringManager
    {
        public static List<string> ExtractAllHrefs(string htmlString)
        {
            List<string> list = new List<string>();
            Regex regex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
            if (regex.IsMatch(htmlString)) foreach (Match match in regex.Matches(htmlString)) if (!match.Groups[1].Value.EndsWith(".png")) list.Add(match.Groups[1].Value);
            return list;
        }
    }
}

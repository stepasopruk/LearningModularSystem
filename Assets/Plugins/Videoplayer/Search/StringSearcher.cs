using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeBase.Utilities.Searching
{
    public static class StringSearcher
    {
        private static Regex GetRegex(string pattern)
        {
            return new Regex($@"{pattern.ToLower()}(\w*)");
        }

        public static IEnumerable<string> GetElementsContain(this IEnumerable<string> stringList, string matchPattern)
        {
            var regex = GetRegex(matchPattern);
            return stringList.Where(x => regex.IsMatch(x.ToLower()));
        }

        public static IEnumerable<ISearchable> GetElementsContain(this IEnumerable<ISearchable> stringList, string matchPattern)
        {
            var regex = GetRegex(matchPattern);
            return stringList.Where(x => regex.IsMatch(x.Text.ToLower()));
        }
    }
}

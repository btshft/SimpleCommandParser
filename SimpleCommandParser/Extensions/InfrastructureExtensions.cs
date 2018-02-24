using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCommandParser.Extensions
{
    internal static class InfrastructureExtensions
    {

        public static string[] PreserveSplit(this string source, string[] splitters)
        {
            for (var i = 0; i < splitters.Length - 1; i++)
            {
                var splitter = splitters[i];
                var parts = source
                    .Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries);

                parts = parts.Length == 1 && parts[0] == source
                    ? parts
                    : parts.Select(p => p.Prepend(splitter)).ToArray();

                source = string.Join(string.Empty, parts);
            }
            
            var lastSplitter = splitters[splitters.Length - 1];
            var lastParts = source
                .Split(new[] { lastSplitter }, StringSplitOptions.RemoveEmptyEntries);
            
            lastParts = lastParts.Length == 1 && lastParts[0] == source
                ? lastParts
                : lastParts.Select(p => p.Prepend(lastSplitter)).ToArray();

            return lastParts;
        }

        public static IEnumerable<string> TrimAll(this IEnumerable<string> source)
        {
            return source.Select(c => c.Trim());
        }

        public static string Escape(this string source, char escape)
        {
            return $"{escape}{source}{escape}";
        }
        
        public static string Prepend(this string source, string prepend)
        {
            return source.Insert(0, prepend);
        }
        
        public static string[] SplitByWhiteSpace(this string source)
        {
            return source.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
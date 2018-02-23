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
                    .Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                parts = parts.Length == 1 && parts[0] == source
                    ? parts
                    : parts.Select(p => p.Prepend(splitter)).ToArray();

                source = string.Join(string.Empty, parts);
            }
            
            var lastSplitter = splitters[splitters.Length - 1];
            var lastParts = source
                .Split(lastSplitter, StringSplitOptions.RemoveEmptyEntries);
            
            lastParts = lastParts.Length == 1 && lastParts[0] == source
                ? lastParts
                : lastParts.Select(p => p.Prepend(lastSplitter)).ToArray();

            return lastParts;
        }

        public static IEnumerable<string> TrimAll(this IEnumerable<string> source)
        {
            return source.Select(c => c.Trim());
        }
        
        public static IEnumerable<string> FlattenJoin(this IEnumerable<IEnumerable<string>> source, char joiner)
        {
            return source.Select(i => string.Join(joiner, i));
        }

        public static IEnumerable<string> PreJoinWith(this IEnumerable<string> source, char prepend)
        {
            return source.Select(s => $"{prepend}{s}");
        }
        
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IReadOnlyCollection<T> source, int size)
        {
            for (var i = 0; i < (float)source.Count / size; i++)
            {
                yield return source.Skip(i * size).Take(size);
            }
        }

        public static string Escape(this string source, char escape)
        {
            return $"{escape}{source}{escape}";
        }
        
        public static string Append(this string source, string append)
        {
            return source.Insert(source.Length, append);
        }
        
        public static string Prepend(this string source, string prepend)
        {
            return source.Insert(0, prepend);
        }
        
        public static string[] SplitByWhiteSpace(this string source)
        {
            return source.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitRemoveEmpty(this string source, string split)
        {
            return source.Split(new[] {split}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] PrependSplit(this string source, string splitter, char? prepend)
        {
            source = prepend.HasValue ? source.Insert(0, prepend.Value.ToString()) : source;          
            return source.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCommandParser.Extensions
{
    internal static class InfrastructureExtensions
    {
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
        
        public static string[] SplitByWhiteSpace(this string source)
        {
            return source.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitRemoveEmpty(this string source, char split)
        {
            return source.Split(new[] {split}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] PrependSplit(this string source, string splitter, char? prepend)
        {
            source = prepend.HasValue ? source.Insert(0, prepend.Value.ToString()) : source;          
            return source.Split(new[] { splitter }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string RemoveFirstChar(this string source)
        {
            return source.Remove(0, 1);
        }

        public static bool IsNullOrWhiteSpace(this char? source)
        {
            return source == null || source.Value == ' ';
        }       
    }
}
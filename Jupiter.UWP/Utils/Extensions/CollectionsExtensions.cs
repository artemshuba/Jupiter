using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Globalization.Collation;

namespace Jupiter.Utils.Extensions
{
    public static class CollectionsExtensions
    {
        /// <summary>
        /// Groups and sorts into a list of alpha groups based on a string selector.
        /// </summary>
        /// <typeparam name="TSource">Type of the items in the list.</typeparam>
        /// <param name="source">List to be grouped and sorted.</param>
        /// <param name="selector">A selector that will provide a value that items to be sorted and grouped by.</param>
        /// <returns>A list of JumpListGroups.</returns>
        public static Dictionary<string, List<TSource>> ToAlphaGroups<TSource>(
            this IEnumerable<TSource> source, Func<TSource, string> selector)
        {
            // Get the letters representing each group for current language using CharacterGroupings class
            var characterGroupings = new CharacterGroupings();

            // Create dictionary for the letters and replace '...' with proper globe icon
            var keys = characterGroupings.Where(x => x.Label.Any())
                .Select(x => x.Label)
                .ToDictionary(x => x);
            keys["..."] = "\uD83C\uDF10";

            // Create groups for each letters
            var groupDictionary = keys.ToDictionary(x => x.Key, v => new List<TSource>());

            // Sort and group items into the groups based on the value returned by the selector
            var query = from item in source
                orderby selector(item)
                select item;

            foreach (var item in query)
            {
                var sortValue = selector(item);
                var key = characterGroupings.Lookup(sortValue);
                if (keys.ContainsKey(key))
                    groupDictionary[key].Add(item);
                else
                    groupDictionary["..."].Add(item);
            }

            return groupDictionary;
        }
    }
}

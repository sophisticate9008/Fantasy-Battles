using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class AffixMerger
{
    public List<string> MergeAffixes(List<string> affixes)
    {
        var groups = affixes
            .Select(ParseAffix)
            .GroupBy(a => a.Template)
            .ToList();

        List<string> merged = new List<string>();

        foreach (var group in groups)
        {
            var first = group.First();
            int valueCount = first.Values.Count;

            if (group.Any(a => a.Values.Count != valueCount))
            {
                merged.AddRange(group.Select(a => RebuildAffix(a)));
                continue;
            }

            List<int> mergedValues = new int[valueCount].ToList();
            foreach (var affix in group)
            {
                for (int i = 0; i < valueCount; i++)
                {
                    mergedValues[i] += affix.Values[i];
                }
            }

            merged.Add(RebuildAffix(group.Key, mergedValues));
        }

        return merged;
    }

    private AffixInfo ParseAffix(string affix)
    {
        var matches = Regex.Matches(affix, @"\d+");
        var values = matches.Cast<Match>().Select(m => int.Parse(m.Value)).ToList();
        string template = Regex.Replace(affix, @"\d+", "{}");
        return new AffixInfo { Template = template, Values = values };
    }

    private string RebuildAffix(AffixInfo affix)
    {
        return RebuildAffix(affix.Template, affix.Values);
    }

    private string RebuildAffix(string template, List<int> values)
    {
        string[] parts = template.Split(new[] { "{}" }, StringSplitOptions.None);
        StringBuilder sb = new();

        for (int i = 0; i < parts.Length; i++)
        {
            sb.Append(parts[i]);
            if (i < values.Count)
            {
                sb.Append(values[i]);
            }
        }

        return sb.ToString();
    }

    private class AffixInfo
    {
        public string Template { get; set;}
        public List<int> Values { get; set;}
    }
    
}





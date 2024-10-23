using System;
using System.Collections.Generic;

public static class CommonExtension
{
    public static List<T> RandomChoices<T>(this List<T> list, int count, bool allowDuplicates = false)
    {
        if (list == null || list.Count == 0)
        {
            throw new ArgumentException("List cannot be null or empty.", nameof(list));
        }

        List<T> selectedItems = new List<T>();
        System.Random rand = new System.Random();

        if (allowDuplicates)
        {
            for (int i = 0; i < count; i++)
            {
                int index = rand.Next(list.Count);
                selectedItems.Add(list[index]);
            }
        }
        else
        {
            if (count > list.Count)
            {
                count = list.Count; // 返回所有元素
            }

            HashSet<int> selectedIndices = new HashSet<int>();

            while (selectedItems.Count < count)
            {
                int index = rand.Next(list.Count);
                if (selectedIndices.Add(index))
                { // 确保不重复
                    selectedItems.Add(list[index]);
                }
            }
        }

        return selectedItems;
    }
}
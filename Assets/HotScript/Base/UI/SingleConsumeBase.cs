using System.Collections.Generic;

public class SingleConsumeBase : ConsumeBase
{
    public virtual string ItemName { get; set; }
    public virtual int ConsumeCount { get; set; }

    public override List<(string itemName, int consumeCount)> ConsumeItemsData
    {
        get
        {
            consumeItemsData.Clear();
            consumeItemsData.Add((ItemName, ConsumeCount));
            return consumeItemsData;
        }
    }
}
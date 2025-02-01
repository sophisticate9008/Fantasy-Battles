using System.Collections.Generic;

public class DamageCalculator
{
    private readonly List<DamageNodeBase> _nodes = new();

    public DamageCalculator AddNode(DamageNodeBase node)
    {
        _nodes.Add(node);
        return this;
    }

    public void Calculate(DamageContext context)
    {
        foreach (var node in _nodes)
        {
            if (!node.Process(context)) break;
        }
    }
}
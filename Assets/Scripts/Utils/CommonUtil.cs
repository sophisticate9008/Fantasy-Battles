using System;
using System.Linq;
using System.Reflection;

public class CommonUtil
{
    public static Type GetTypeByName(string tyoeName,string extraStr = "")
    {
        var assembly = Assembly.GetExecutingAssembly();
        var type = assembly.GetTypes()
            .FirstOrDefault(t => t.Name.Equals(tyoeName + extraStr, StringComparison.OrdinalIgnoreCase));

        return type ?? throw new NotImplementedException();
    }

}
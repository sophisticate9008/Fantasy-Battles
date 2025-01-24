using System.Reflection;

public class ReflectiveBase
{
    /// <summary>
    /// 根据字段或属性名称获取对应的值。
    /// </summary>
    /// <param name="name">字段或属性的名称。</param>
    /// <returns>返回字段或属性的值，如果找不到返回 null。</returns>
    public object GetFieldValue(string name)
    {
        // 优先尝试获取属性值
        PropertyInfo property = this.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
        if (property != null)
        {
            return property.GetValue(this);
        }

        // 如果没有找到属性，再尝试获取字段值
        FieldInfo field = this.GetType().GetField(name, BindingFlags.Public | BindingFlags.Instance);
        if (field != null)
        {
            return field.GetValue(this);
        }

        // 找不到对应的字段或属性
        return null;
    }
}
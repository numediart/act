using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace act_server.Enum;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>> where TEnum : Enumeration<TEnum>
{
    public static Dictionary<int, TEnum> Enumerations = CreateEnumerations();
    public int Value { get; protected set; }
    public string Name { get; protected set; } = string.Empty;

    protected Enumeration(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public static TEnum FromValue(int value)
    {
        return Enumerations.TryGetValue(value, out TEnum? enumeration)? enumeration : default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(enumeration => enumeration.Name == name) ?? default;
    }


    public bool Equals(Enumeration<TEnum> other)
    {
        if (other is null)
            return false;
        return GetType() == other.GetType() && Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is not Enumeration<TEnum> other)
            return false;
        return Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationsType = typeof(TEnum);
        var fields = enumerationsType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumerationsType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => fieldInfo.GetValue(default)).Cast<TEnum>();
        return fields.ToDictionary(enumeration => enumeration.Value);
    }
    
}
namespace IoTGateway.Models;

public class Reference : IEquatable<Reference>
{
    public Reference(string name)
    {
        Name = name;
    }
    
    public string Name { get; set; }
    
    public static implicit operator Reference(string name)
    {
        return new Reference(name);
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        var reference = (Reference)obj;
        return Name == reference.Name;
    }

    public bool Equals(Reference? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
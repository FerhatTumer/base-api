using System.Collections;

namespace TaskManagement.Domain.Common;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        ValueObject other = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        IEnumerator<object> enumerator = GetAtomicValues().GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return 0;
        }

        int hashCode = enumerator.Current?.GetHashCode() ?? 0;
        while (enumerator.MoveNext())
        {
            int currentHash = enumerator.Current?.GetHashCode() ?? 0;
            hashCode ^= currentHash;
        }

        return hashCode;
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}
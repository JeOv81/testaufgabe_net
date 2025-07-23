namespace Domain.Interfaces;

public interface IEntity<TType> where TType : struct, IComparable, IComparable<TType>, IEquatable<TType>
{
    TType Id { get; set; }
}

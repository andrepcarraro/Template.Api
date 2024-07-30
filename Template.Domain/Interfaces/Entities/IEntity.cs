namespace Template.Domain;
public interface IEntity<TId>
{
    TId Id { get; set; }
}

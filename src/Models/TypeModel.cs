namespace Models;

public class TypeModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
}
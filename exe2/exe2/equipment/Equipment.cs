namespace exe2.equipment;

public abstract class Equipment
{
    public abstract string Id { get; }
    public abstract string Name { get; }
    public abstract string Producer { get; }
    public abstract string Description { get; }

    public bool IsAvailable { get; set; } = true;
}

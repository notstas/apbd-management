namespace exe2.equipment;

public class Projector(string id, string producer, string description) : Equipment
{
    public override string Id { get; } = id;
    public override string Name => "Projector";
    public override string Producer { get; } = producer;
    public override string Description { get; } = description;
}

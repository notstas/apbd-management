namespace exe2.equipment;

public class Projector(string id, string producer, string description) : IEquipment
{
    public string Id { get; } = id;
    string IEquipment.Name => "Projector";
    public string Producer { get; } = producer;
    public string Description { get; } = description;
}
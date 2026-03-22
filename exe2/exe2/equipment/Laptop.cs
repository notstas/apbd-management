namespace exe2.equipment;

public class Laptop(string id, string producer, string description) : IEquipment
{
    public string Id { get; } = id;
    string IEquipment.Name => "Laptop";
    public string Producer { get; } = producer;
    public string Description { get; } = description;
}
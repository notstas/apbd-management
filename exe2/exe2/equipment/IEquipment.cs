namespace exe2.equipment;

public interface IEquipment
{
    string Id { get; }
    string Name { get; }
    
    string Producer { get;  }
    
    string Description { get; }
}
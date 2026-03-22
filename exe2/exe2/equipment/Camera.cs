namespace exe2.equipment;

public class Camera(string id, string resolution, string producer) : IEquipment
{
    private readonly string _resolution = resolution;

    public string Name => "Camera";
    public string Producer { get; } = producer;
    public string Id { get; } = id;
    
    public string Description => $"Camera (ID: {Id}; Producer: {Producer}) {_resolution}";
}
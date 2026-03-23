namespace exe2.equipment;

public class Camera(string id, string resolution, string producer) : Equipment
{
    private readonly string _resolution = resolution;

    public override string Name => "Camera";
    public override string Producer { get; } = producer;
    public override string Id { get; } = id;

    public override string Description => $"Camera (ID: {Id}; Producer: {Producer}) {_resolution}";

     public bool IsAvailable { get; set; } = true;
}

using Interstellar.Messaging;

public class ThingModified : IEvent
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }

    public ThingModified(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
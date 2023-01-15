namespace Interstellar.TestApp.Messages;

public class ThingCreated
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }

    public ThingCreated(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
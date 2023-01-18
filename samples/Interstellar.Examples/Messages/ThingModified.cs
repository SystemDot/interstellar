namespace Interstellar.Examples.Messages;

using MediatR;

public class ThingModified : INotification
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
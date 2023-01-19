namespace Interstellar.Examples.Messages;

public class CreateOrModifyThing
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }

    public CreateOrModifyThing(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
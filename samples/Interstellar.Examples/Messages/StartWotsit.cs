namespace Interstellar.Examples.Messages;

public class StartWotsit
{
    public Guid Id { get;  }
    public Guid ThingId { get;  }

    public StartWotsit(Guid id, Guid thingId)
    {
        Id = id;
        ThingId = thingId;
    }
}
namespace Interstellar.Examples.Messages;

public class MakeWotsit
{
    public Guid Id { get;  }
    public Guid ThingId { get;  }
    public decimal Cost { get;  }

    public MakeWotsit(Guid id, Guid thingId, decimal cost)
    {
        Id = id;
        ThingId = thingId;
        Cost = cost;
    }
}
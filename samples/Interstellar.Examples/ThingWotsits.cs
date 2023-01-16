using Interstellar.Configuration;
using Interstellar.Examples.Messages;

namespace Interstellar.Examples;

public class ThingWotsits : AggregateRoot
{
    public static void Configure(DomainConfiguration configuration)
    {
        configuration
            .ForAggregate<ThingWotsits>()
                .ReceiveCommand<StartWotsit>(command => command.Id.ToThingWotsitStreamId())
                    .JoinWithOtherStreams(command => command.Id.ToThingStreamId())
                .ReceiveCommand<MakeWotsit>(command => command.Id.ToThingWotsitStreamId())
                    .JoinWithOtherStreams(command => command.Id.ToThingStreamId())
                .ReceiveCommand<DestroyWotsit>(command => command.Id.ToThingWotsitStreamId())
                    .JoinWithOtherStreams(command => command.Id.ToThingStreamId());
    }

    private readonly UserService userService;

    public ThingWotsits(UserService userService)
    {
        this.userService = userService;

        On<ThingCreated>().Become(WotsitNotYetStarted);
        On<WotsitStarted>().Become(WotsitStarted);
        On<WotsitMade>().Become(WotsitMade);
        On<WotsitDestroyed>().Become(WotsitNotYetStarted);
        ThingNotCreatedYet();
    }

    private void ThingNotCreatedYet()
    {
    }

    private void WotsitNotYetStarted()
    {
        When<StartWotsit>().DoAsync(async command =>
        {
            var currentUser = await userService.GetCurrentUserAsync();
            Then(new WotsitStarted(command.ThingId, currentUser.Id, DateTime.UtcNow));
        });
    }

    private void WotsitStarted()
    {
        When<MakeWotsit>().DoAsync(async command =>
        {
            var currentUser = await userService.GetCurrentUserAsync();
            Then(new WotsitMade(command.ThingId, currentUser.Id, DateTime.UtcNow, command.Cost));
        });
    }

    private void WotsitMade()
    {
        When<DestroyWotsit>().DoAsync(async command =>
        {
            var currentUser = await userService.GetCurrentUserAsync();
            Then(new WotsitDestroyed(command.ThingId, currentUser.Id, DateTime.UtcNow));
        });
    }
}
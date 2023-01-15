using Interstellar.Domain;
using Interstellar.Domain.Configuration;
using Interstellar.TestApp.Messages;

namespace Interstellar.TestApp;

public class ThingWotsits : AggregateRoot
{
    public static void Configure(DomainConfiguration configuration, UserService userService)
    {
        configuration
            .Route<StartWotsit>()
                .ToAggregate(() => new ThingWotsits(userService))
                .ForId(command => $"{nameof(Thing)}-{command.Id}")
            .Route<MakeWotsit>()
                .ToAggregate(() => new ThingWotsits(userService))
                .ForId(command => $"{nameof(Thing)}-{command.Id}")
            .Route<DestroyWotsit>()
                .ToAggregate(() => new ThingWotsits(userService))
                .ForId(command => $"{nameof(ThingWotsits)}-{command.Id}");
    }

    private readonly UserService userService;

    private ThingWotsits(UserService userService)
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
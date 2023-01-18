using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;

namespace Interstellar.EventStorage.CosmosDb;

public class CosmosDatabaseInitialiser
{
    private readonly IEventSourcingCosmosContainerProvider containerProvider;

    public CosmosDatabaseInitialiser(IEventSourcingCosmosContainerProvider containerProvider)
    {
        this.containerProvider = containerProvider;
    }

    public async Task InitialiseAsync()
    {
        if (await StoredProceduresExistAsync())
        {
            await DeleteStoredProceduresAsync();
        }
        await CreateStoredProceduresAsync();
    }

    private async Task<bool> StoredProceduresExistAsync()
    {
        var container = await GetContainerAsync();
        var iterator = container.Scripts.GetStoredProcedureQueryIterator<StoredProcedureProperties>();
        var procedures = await iterator.ReadNextAsync();
        return procedures.Any();
    }

    private async Task DeleteStoredProceduresAsync()
    {
        await DeleteStoredProcedureAsync(StoredProcedures.EventStorage);
        await DeleteStoredProcedureAsync(StoredProcedures.RemoveImmediateDispatchPosition);
    }

    private async Task CreateStoredProceduresAsync()
    {
        await CreateStoredProcedureAsync(StoredProcedures.EventStorage);
        await CreateStoredProcedureAsync(StoredProcedures.RemoveImmediateDispatchPosition);
    }

    private async Task DeleteStoredProcedureAsync(string procedureId)
    {
        var container = await GetContainerAsync();
        await container.Scripts.DeleteStoredProcedureAsync(procedureId);
    }

    private async Task CreateStoredProcedureAsync(string procedureId)
    {
        var body = EmbeddedResourceReader.Read($"Interstellar.EventStorage.CosmosDb.{procedureId}.js");
        var props = new StoredProcedureProperties
        {
            Id = procedureId,
            Body = body
        };

        var container = await GetContainerAsync();
        await container.Scripts.CreateStoredProcedureAsync(props);
    }

    private Task<Container> GetContainerAsync() =>
        containerProvider.ProvideContainerAsync(PartitionKeys.StreamId);
}
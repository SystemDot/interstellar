namespace Interstellar.Tests.Performance;

public static class GuidExtensions
{
    public static string ToPerformanceTestStreamId(this Guid id) => $"{nameof(PerformanceTestAggregate)}-{id}";
}
namespace Interstellar.Tests.Performance;

public class TestRunner
{
    private readonly MessageBus messageBus;

    public TestRunner(MessageBus messageBus)
    {
        this.messageBus = messageBus;
    }

    public async Task RunAsync(int eventCount, int runCount)
    {
        var samples = new List<TimeSpan>();

        int sampleInterval = 1;

        if (runCount > 3)
        {
            sampleInterval = runCount / 3;
        }

        for (int i = 0; i < runCount; i++)
        {
            var startTime = DateTime.Now;
            await messageBus.SendCommandAsync(new TestPerformanceCommand(Guid.NewGuid(), eventCount));

            var endTime = DateTime.Now;

            if (i % sampleInterval == 0)
            {
                samples.Add(endTime - startTime);
            }
        }

        var sampleNumber = 1;
        foreach (var sample in samples)
        {
            Console.WriteLine($"Sample #{sampleNumber} time: {sample.Milliseconds}ms ");
            sampleNumber++;
        }
    }
}
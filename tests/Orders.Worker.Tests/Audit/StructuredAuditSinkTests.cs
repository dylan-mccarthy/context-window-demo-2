using Microsoft.Extensions.Logging;
using Orders.Worker.Audit;

namespace Orders.Worker.Tests.Audit;

public sealed class StructuredAuditSinkTests
{
    [Fact]
    public async Task WriteAsync_LogsEventAsStructuredData()
    {
        var logger = new RecordingLogger<StructuredAuditSink>();
        var sink = new StructuredAuditSink(logger);

        await sink.WriteAsync(
            new AuditEvent("payment.accepted", "order-123", new { paymentId = "pay-456", amount = 42.50m }),
            CancellationToken.None);

        var message = Assert.Single(logger.Messages);
        Assert.Contains("payment.accepted", message);
        Assert.Contains("order-123", message);
        Assert.Contains("pay-456", message);
        Assert.Contains("42.50", message);
    }

    [Fact]
    public async Task WriteAsync_HonorsCancellation()
    {
        var sink = new StructuredAuditSink(new RecordingLogger<StructuredAuditSink>());

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            sink.WriteAsync(
                new AuditEvent("order.created", "order-123", new { customerId = "customer-456" }),
                new CancellationToken(canceled: true)));
    }

    private sealed class RecordingLogger<T> : ILogger<T>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) => Messages.Add(formatter(state, exception));
    }
}
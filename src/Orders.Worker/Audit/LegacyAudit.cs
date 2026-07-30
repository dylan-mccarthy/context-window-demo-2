namespace Orders.Worker.Audit;

[Obsolete("Use IAuditSink.WriteAsync instead.")]
public static class LegacyAudit
{
    private static readonly AsyncLocal<ICollection<AuditEvent>?> CapturedEvents = new();

    public static void Write(string eventName, string correlationId, object payload)
    {
        CapturedEvents.Value?.Add(new AuditEvent(eventName, correlationId, payload));
    }

    public static IDisposable Capture(ICollection<AuditEvent> events)
    {
        var previous = CapturedEvents.Value;
        CapturedEvents.Value = events;
        return new CaptureScope(() => CapturedEvents.Value = previous);
    }

    private sealed class CaptureScope(Action dispose) : IDisposable
    {
        public void Dispose() => dispose();
    }
}
using Orders.Worker.Audit;
using Orders.Worker.Handlers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IAuditSink, StructuredAuditSink>();
builder.Services.AddTransient<OrderCreatedHandler>();
builder.Services.AddTransient<PaymentAcceptedHandler>();
builder.Services.AddTransient<PaymentRejectedHandler>();
builder.Services.AddTransient<OrderPackedHandler>();
builder.Services.AddTransient<OrderShippedHandler>();
builder.Services.AddTransient<OrderCancelledHandler>();
builder.Services.AddTransient<RefundRequestedHandler>();
builder.Services.AddTransient<RefundCompletedHandler>();

var host = builder.Build();
host.Run();

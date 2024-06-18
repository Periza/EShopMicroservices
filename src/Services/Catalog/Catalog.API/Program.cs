var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(configuration: config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddMarten(configure: opts =>
{
    opts.Connection(connectionString: builder.Configuration.GetConnectionString(name: "Database")!);
}).UseLightweightSessions();

var app = builder.Build();

// Configure the HTTP request pipeline

app.MapCarter();

app.Run();
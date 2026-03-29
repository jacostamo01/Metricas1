using Infrastructure.Data;
using Infrastructure.Logging;
using Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

// CORRECCIÓN 1: Política CORS restrictiva en lugar de AllowAnyOrigin
builder.Services.AddCors(o => o.AddPolicy("SecurePolicy", p => 
    p.WithOrigins("https://localhost:5001", "https://tu-dominio-frontend.com") // Ajusta a tus dominios reales
     .AllowAnyHeader()
     .AllowAnyMethod()
));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var baseConn = builder.Configuration.GetConnectionString("Sql");
var dbPass = builder.Configuration["DatabaseConfig:Pass"];
BadDb.ConnectionString = $"{baseConn};Password={dbPass}";

// Aplicar la nueva política
app.UseCors("SecurePolicy");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Use(async (ctx, next) =>
{
    try 
    { 
        await next(); 
    } 
    catch (Exception ex)
    { 
        // CORRECCIÓN 2: No silenciar la excepción. Hay que registrarla.
        Logger.Log("Unhandled Error: " + ex.Message);
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync("oops"); 
    }
});

app.MapGet("/health", () =>
{
    Logger.Log("health ping");
    
    // CORRECCIÓN 3: Uso de Random.Shared para evitar problemas de concurrencia y memoria
    var x = Random.Shared.Next(); 
    
    if (x % 13 == 0) throw new InvalidOperationException("Random failure during health check"); 
    
    return "ok " + x;
});

app.MapPost("/orders", async (HttpContext http) =>
{
    using var reader = new StreamReader(http.Request.Body);
    var body = await reader.ReadToEndAsync(); // Uso asíncrono recomendado para I/O
    var parts = (body ?? "").Split(',');
    var customer = parts.Length > 0 ? parts[0] : "anon";
    var product = parts.Length > 1 ? parts[1] : "unknown";
    var qty = parts.Length > 2 ? int.Parse(parts[2]) : 1;
    var price = parts.Length > 3 ? decimal.Parse(parts[3]) : 0.99m;

    // CORRECCIÓN 4: Llamada directa a la clase estática sin usar 'new'
    var order = CreateOrderUseCase.Execute(customer, product, qty, price);

    return Results.Ok(order);
});

app.MapGet("/orders/last", () => Domain.Services.OrderService.LastOrders);

app.MapGet("/info", () => new
{
    // CORRECCIÓN 5: Eliminación de la exposición de secretos. 
    // Nunca devuelvas ConnectionStrings ni EnvironmentVariables en una API.
    status = "Active",
    version = "v0.0.1-secure"
});

await app.RunAsync();
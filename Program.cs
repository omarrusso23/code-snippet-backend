using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();
Console.WriteLine($"Base URL: {Environment.GetEnvironmentVariable("SUPABASE_BASE_URL")}");
Console.WriteLine($"API Key: {Environment.GetEnvironmentVariable("SUPABASE_API_KEY")}");


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<SuperbaseServices>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendApp", policy =>
    {
        policy.WithOrigins("https://code-snippet-sharing-web.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Enable CORS before other middlewares
app.UseCors("AllowFrontendApp");

// Comment out or remove this line if you're not using HTTPS
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

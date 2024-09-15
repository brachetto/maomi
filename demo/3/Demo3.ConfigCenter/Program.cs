using Demo3.ConfigCenter.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ע�� SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<ConfigCenterHub>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

// ���� Hub �м��
app.MapHub<ConfigCenterHub>("/config");
app.Run("http://*:5000");

using Maomi.Web.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 1������ע��
builder.Services.AddSwaggerGen(options =>
{
	// ģ���������
	options.SchemaFilter<MaomiSwaggerSchemaFilter>();
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	// 2�����������м��
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

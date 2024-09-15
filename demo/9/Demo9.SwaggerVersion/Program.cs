using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// ���� Api �汾��Ϣ
builder.Services.AddApiVersioning(setup =>
{
	// ȫ��Ĭ�� api �汾��
	setup.DefaultApiVersion = new ApiVersion(1, 0);
	// �û�����δָ���汾��ʱ��ʹ��Ĭ�ϰ汾��
	setup.AssumeDefaultVersionWhenUnspecified = true;
	// ��Ӧʱ���� header �з��ذ汾��
	setup.ReportApiVersions = true;
	// �������ȡ�汾����Ϣ
	setup.ApiVersionReader =
	ApiVersionReader.Combine(
	   new HeaderApiVersionReader("X-Api-Version"),
	   new QueryStringApiVersionReader("version"));
});

// �� swagger ����ʾ�汾��Ϣ��
// ��һ��ʹ�ð汾�Ž��и���
builder.Services.AddVersionedApiExplorer(o =>
{
	// ��ȡ�����ð汾������ url ��ַ��
	o.SubstituteApiVersionInUrl = true;
	// swagger ҳ��Ĭ������İ汾��
	o.DefaultApiVersion = new ApiVersion(1, 0);
	// ��ʾ�İ汾�����ʽ
	o.GroupNameFormat = "'v'VVV";
});

builder.Services.AddSwaggerGen(options =>
{
	var ioc = builder.Services.BuildServiceProvider();
	var apiVersionDescriptionProvider = ioc.GetRequiredService<IApiVersionDescriptionProvider>();
	var apiVersionoptions = ioc.GetRequiredService<IOptions<ApiVersioningOptions>>();
	foreach (var item in apiVersionDescriptionProvider.ApiVersionDescriptions)
	{
		// ��ÿ���汾�Ŵ��� swagger.json 
		options.SwaggerDoc(item.GroupName, new OpenApiInfo
		{
			Version = apiVersionoptions.Value.DefaultApiVersion.ToString(),
			Title = item.GroupName,
		});
	}
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	// ���� ui
	app.UseSwaggerUI(options =>
	{
		var ioc = app.Services;
		var apiVersionDescriptionProvider = ioc.GetRequiredService<IApiVersionDescriptionProvider>();
		var descriptions = apiVersionDescriptionProvider.ApiVersionDescriptions;

		// Build a swagger endpoint for each discovered API version
		foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
		{
			var url = $"/swagger/{description.GroupName}/swagger.json";
			var name = description.GroupName.ToUpperInvariant();
			options.SwaggerEndpoint(url, name);
		}
	});
}

app.UseAuthorization();

app.MapControllers();

app.Run();

using Maomi;
using Maomi.Web.Core;

namespace MaomiDemo.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllers();

			// ע��ģ�黯���񣬲����� ApiModule Ϊ���
			builder.Services.AddModule<ApiModule>();
			//  swagger ����
			builder.Services.AddMaomiSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				// swagger �м��
				app.UseMaomiSwagger();
			}

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
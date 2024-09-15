using Microsoft.AspNetCore.Mvc;

namespace Demo9.SwaggerVersion.Controllers
{
	/// <summary>
	/// ������A
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class AController : ControllerBase
	{
		/// <summary>
		/// ����
		/// </summary>
		/// <returns></returns>
		[HttpGet("test1")]
		public string Get1() => "true";
	}

	/// <summary>
	/// ������B
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[ApiVersion("1.0")]
	public class BController : ControllerBase
	{
		/// <summary>
		/// ����
		/// </summary>
		/// <returns></returns>
		[HttpGet("test1")]
		public string Get1() => "true";
	}

	/// <summary>
	/// ������C
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	[ApiVersion("2.0")]
	public class CController : ControllerBase
	{
		/// <summary>
		/// ����
		/// </summary>
		/// <returns></returns>
		[HttpGet("test1")]
		public string Get1() => "true";
	}
}
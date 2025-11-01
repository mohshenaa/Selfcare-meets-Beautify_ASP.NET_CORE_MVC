namespace Selfcare_meets_Beautify.Services
{
	public static class ServiceHelper
	{
		public static void AddFileUploader(this IServiceCollection services)
		{
			services.AddScoped<IUploadService, UploadService>();
		}
	}
}

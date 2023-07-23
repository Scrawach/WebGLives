using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace WebGLives.API.Extensions;

public static class BuilderExtensions
{
    public static IApplicationBuilder UseGameStorage(this IApplicationBuilder app)
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings[".unityweb"] = "application/octet-stream";
        var staticFilesPath = Path.Combine(Path.GetTempPath(), "WebGLives");
        
        if (!Directory.Exists(staticFilesPath))
            Directory.CreateDirectory(staticFilesPath);
        
        var fileProvider = new PhysicalFileProvider(staticFilesPath);

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = fileProvider,
            ContentTypeProvider = contentTypeProvider
        });

        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = fileProvider,
        });

        return app;
    }
}
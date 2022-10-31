using System.IO;
using System.Reflection;

namespace VirtualNetworkAdapterExample.Services;

public class ResourceService
{
    private readonly Assembly _assembly;

    public ResourceService()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public void ExtractResource(string resourceName, string fileName)
    {
        var fileInfo = new FileInfo(fileName);
        var directory = fileInfo.Directory?.FullName;
        if (directory != null && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using var resource =
            _assembly.GetManifestResourceStream($"VirtualNetworkAdapterExample.Resources.{resourceName}");
        using var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
        resource?.CopyTo(fs);
    }
}
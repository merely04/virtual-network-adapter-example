using System.Diagnostics;
using System.Threading.Tasks;
using VirtualNetworkAdapterExample.Services;

namespace VirtualNetworkAdapterExample.Commands;

public class RemoveDeviceCommand : AsyncCommandBase
{
    private readonly HotspotService _hotspotService;

    public RemoveDeviceCommand(HotspotService hotspotService)
    {
        _hotspotService = hotspotService;
    }

    protected override async Task ExecuteAsync(object? parameter)
    {
        if (parameter is string id)
            _hotspotService.Remove(id);
    }
}
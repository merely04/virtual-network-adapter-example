using System.Diagnostics;
using System.Threading.Tasks;
using VirtualNetworkAdapterExample.Services;

namespace VirtualNetworkAdapterExample.Commands;

public class CreateDeviceCommand : AsyncCommandBase
{
    private readonly HotspotService _hotspotService;

    public CreateDeviceCommand(HotspotService hotspotService)
    {
        _hotspotService = hotspotService;
    }

    protected override async Task ExecuteAsync(object? parameter)
    {
        _hotspotService.Install();
    }
}
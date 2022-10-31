using System.Threading.Tasks;
using VirtualNetworkAdapterExample.Services;

namespace VirtualNetworkAdapterExample.Commands;

public class UpdateDeviceListCommand : AsyncCommandBase
{
    private readonly HotspotService _hotspotService;

    public UpdateDeviceListCommand(HotspotService hotspotService)
    {
        _hotspotService = hotspotService;
    }

    protected override async Task ExecuteAsync(object? parameter)
    {
        _hotspotService.DeviceListUpdated?.Invoke();
    }
}
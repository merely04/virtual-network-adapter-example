using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VirtualNetworkAdapterExample.Commands;
using VirtualNetworkAdapterExample.Models;
using VirtualNetworkAdapterExample.Services;

namespace VirtualNetworkAdapterExample.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly HotspotService _hotspotService;

    private ObservableCollection<Device> _devices;
    public IEnumerable<Device> Devices => _devices;

    private Device? _selectedDevice;

    public Device? SelectedDevice
    {
        get => _selectedDevice;
        set
        {
            SetField(ref _selectedDevice, value);
            OnPropertyChanged(nameof(CanRemoved));
        }
    }

    public bool CanRemoved => _selectedDevice != null;

    public ICommand CreateDeviceCommand { get; }
    public ICommand RemoveDeviceCommand { get; }
    
    public ICommand UpdateDeviceListCommand { get; }

    public MainViewModel(
        HotspotService hotspotService,
        CreateDeviceCommand createDeviceCommand,
        RemoveDeviceCommand removeDeviceCommand,
        UpdateDeviceListCommand updateDeviceListCommand
    )
    {
        _hotspotService = hotspotService;
        _devices = new ObservableCollection<Device>(_hotspotService.FindAll());

        _hotspotService.DeviceListUpdated += DeviceListUpdated;

        CreateDeviceCommand = createDeviceCommand;
        RemoveDeviceCommand = removeDeviceCommand;
        UpdateDeviceListCommand = updateDeviceListCommand;
    }

    private void DeviceListUpdated()
    {
        _devices = new ObservableCollection<Device>(_hotspotService.FindAll());
        OnPropertyChanged(nameof(Devices));
    }

    public override void Dispose()
    {
        _hotspotService.DeviceListUpdated -= DeviceListUpdated;
    }
}
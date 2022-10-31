using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Windows;
using VirtualNetworkAdapterExample.Models;

namespace VirtualNetworkAdapterExample.Services;

public class HotspotService
{
    private readonly string _fileName;

    private readonly ProcessStartInfo _ps;

    public Action? DeviceListUpdated;

    public HotspotService(ResourceService resourceService, string workingDirectory)
    {
        var file = "devcon.exe";
        _fileName = Path.Combine(workingDirectory, "devcon.exe");
        
        resourceService.ExtractResource(file, _fileName);
        
        _ps = new ProcessStartInfo()
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            FileName = "cmd"
        };
    }

    public void Install()
    {
        _ps.Arguments = @"-r install %windir%\inf\netloop.inf *msloop";
        Execute(_ps);

        DeviceListUpdated?.Invoke();
    }

    public IEnumerable<Device> FindAll()
    {
        var result = new List<Device>();

        var searcher = new ManagementObjectSearcher(
            "SELECT Name, NetConnectionID, PNPDeviceID, ConfigManagerErrorCode, NetConnectionStatus " +
            "FROM Win32_NetworkAdapter " +
            "WHERE ServiceName='kmloop'"
        );
        foreach (var oReturn in searcher.Get())
        {
            var properties = oReturn.Properties;
            if (properties["NetConnectionID"].Value != null)
                result.Add(new Device(properties["PNPDeviceID"].Value.ToString(),
                    properties["NetConnectionID"].Value.ToString()));
        }

        return result;
    }

    public void Remove(string id)
    {
        _ps.Arguments = $"-r remove @{id}";
        Execute(_ps);

        DeviceListUpdated?.Invoke();
    }

    public void RemoveAll()
    {
        _ps.Arguments = $"-r remove *msloop";
        Execute(_ps);
    }

    private void Execute(ProcessStartInfo ps)
    {
        try
        {
            ps.Arguments = $"/c \"{_fileName}\" {ps.Arguments}";
            using var p = Process.Start(ps);

            p?.WaitForExit();
            var output = p?.StandardOutput.ReadToEnd();

            var error = p?.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
                throw new Exception(error.TrimEnd());

            Debug.WriteLine($"Output: {output}");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
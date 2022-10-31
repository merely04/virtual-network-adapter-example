namespace VirtualNetworkAdapterExample.Models;

public class Device
{
    public string? Id { get; }
    public string? Name { get; }

    public Device(string? id, string? name)
    {
        Id = id;
        Name = name;
    }
}
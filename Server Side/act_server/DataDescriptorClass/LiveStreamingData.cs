using System;

namespace act_server.DataDescriptorClass;

public class LiveStreamingData
{
    private string message;

    public LiveStreamingData(string message)
    {
        this.message = message;
        Console.WriteLine(this.message);
    }

}
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");


// Simlate High CPU Usage
app.MapGet("/cpu", () =>
{
    while (true)
    {
        // Busy loop to consume CPU
        Math.Pow(12345, 67890);
    }
});

// Simulate Out of Memory (OOM)
app.MapGet("/oom", () =>
{
    var list = new List<byte[]>();
    while (true)
    {
        list.Add(new byte[1024 * 1024]); // 1 MB allocations
        Thread.Sleep(10);
    }
});

// Simulate Socket/Port Exhaustion
app.MapGet("/sockets", async () =>
{
    var tasks = new List<Task>();
    for (int i = 0; i < 10000; i++)
    {
        tasks.Add(Task.Run(async () =>
        {
            using var client = new HttpClient();
            try
            {
                await client.GetAsync("https://example.com");
            }
            catch { /* ignore errors */ }
        }));
    }
    await Task.WhenAll(tasks);
});


// Simulate File I/O Bottlenecks
app.MapGet("/io", () =>
{
    for (int i = 0; i < 10000; i++)
    {
        File.WriteAllText($"temp_{i}.txt", new string('x', 1000));
    }
    return "Disk write done.";
});


// Simulate Thread Pool Starvation
app.MapGet("/threads", () =>
{
    for (int i = 0; i < 1000; i++)
    {
        ThreadPool.QueueUserWorkItem(_ =>
        {
            Thread.Sleep(10000); // Long blocking call
        });
    }
    return "Started blocking tasks.";
});


app.Run();
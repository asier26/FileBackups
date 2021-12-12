using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly IConfiguration config;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        this.logger = logger;
        this.config = config;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            try
            {
                CopyFiles();
                logger.Log(LogLevel.Information, $"File copied at: {DateTime.Now}");

            }catch(Exception ex)
            {
                logger.Log(LogLevel.Error, $"File don´t copy at: {DateTime.Now}/nError: {ex}");
            }
            await Task.Delay(new TimeSpan(0, config.GetValue<int>("TimeInMinutes"), 0), stoppingToken);
        }
    }

    private void CopyFiles()
    {
        string sourDir = config.GetValue<string>("SourceDir");
        string bDir = config.GetValue<string>("BackupDir");
        string[] fileList = Directory.GetFiles(sourDir, "*." + config.GetValue<string>("FileExt"));
        
        // Copy files.
        foreach (string file in fileList)
        {
            // Remove path from the file name.
            string fName = file.Substring(sourDir.Length + 1);

            // Use the Path.Combine method to safely append the file name to the path.
            int n = CheckFiles(fName);
            File.Copy(Path.Combine(sourDir, fName), Path.Combine(bDir, fName + $"-copy.{n}"));
        }
    }

    private int CheckFiles(string fName)
    {
        int copies = config.GetValue<int>("NumOfMaxCopies");
        string bDir = config.GetValue<string>("BackupDir");

        // Check the copy number of files
        for(int i = 1; i <= copies; i++)
        {
            if(!File.Exists(Path.Combine(bDir, fName + $"-copy.{i}")))
                return i;
        }

        // If reached max, rename the files and return the max number
        File.Delete(Path.Combine(bDir, fName + "-copy.1"));

        for(int i = 1; i < copies; i++)
        {
            File.Move(Path.Combine(bDir, fName + $"-copy.{i+1}"), (Path.Combine(bDir, fName + $"-copy.{i}")));
        }
        return copies;
    }
}
FileBackups
===
This aplication aims to create unlimited copies of the files you want.
FileBackups is built on [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).

To start FileBackup aplication you have install Microsoft.Extensions packages:
```
dotnet add package Microsoft.Extensions.Hosting
```

Before you have configure appsettings.json file, you can define "TimeInMinutes" property to get a copy of the file each 5 minutes and "NumOfMaxCopies" property to set a maximun number of copies, for example:
```
{
    "SourceDir": "C:\\Users\\YourUserName\\Images",
    "BackupDir": "C:\\Users\\YourUserName\\Images\\BackupDirectory",
    "FileExt": "jpg",
    "TimeInMinutes": 5,
    "NumOfMaxCopies": 5
}
```
Lastly, for Debug, simply build and run the aplication:
```
dotnet build
dotnet run
```

Publish
---
To publish the aplication as a Service on Windows, you have to add a "WindowsServices" package and release the app:
```
dotnet add package Microsoft.Extensions.Hosting.WindowsServices
dotnet publish --configuration Release
```
To register this aplication as a service on Windows, you can use the "SC" tool which is located in C:\Windows\system32. To be able to run this tool you have to execute "Powersell" with administrator privileges. And execute the following command:
```
.\sc.exe create ServiceName binPath=C:\"Program Files"\FileBackups
```
In binPath parameter you have to define your host aplication route.
To run the service, first go to "Services" aplication and search for the "ServiceName". There, you can run it or set it to run automatically. 

License
---
This console aplication is under the MIT License.
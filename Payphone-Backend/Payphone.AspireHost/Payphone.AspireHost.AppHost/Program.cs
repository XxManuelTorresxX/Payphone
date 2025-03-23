using Aspire.Hosting;
using System.IO;

var builder = DistributedApplication.CreateBuilder(args);

var projectPath = Path.Combine("..", "..", "Payphone.Api", "Payphone.Api.csproj");
builder.AddProject("PayphoneApi", projectPath, "http");

builder.Build().Run();

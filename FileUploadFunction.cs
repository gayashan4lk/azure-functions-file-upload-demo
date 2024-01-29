using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;

namespace FileUploadDemo;

public class FileUploadFunction
{
    private readonly ILogger<FileUploadFunction> _logger;

    public FileUploadFunction(ILogger<FileUploadFunction> logger)
    {
        _logger = logger;
    }

    [Function("fileUpload")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
     
        string ConnectionString = Environment.GetEnvironmentVariable("MY_STORAGE_CONNECTION");
        string ContainerName = Environment.GetEnvironmentVariable("MY_STORAGE_CONTAINER_NAME");

        //_logger.LogInformation("C# HTTP trigger function processed a request.");

        //string name = req.Query["name"];

        //string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

        //dynamic data = JsonSerializer.Deserialize<dynamic>(reqBody);

        //name = name ?? data?.name;

        //string responseMessage = string.IsNullOrEmpty(name) ? "Pass a name in the query string" : $"Hello, {name}";

        //return new OkObjectResult(responseMessage);

        Stream myBlob = new MemoryStream();
        var file = req.Form.Files["file"];
        myBlob = file.OpenReadStream();
        var blobClient = new BlobContainerClient(ConnectionString, ContainerName);
        var blob = blobClient.GetBlobClient(file.FileName);
        await blob.UploadAsync(myBlob);
        return new OkObjectResult("File uploaded");
    }
}

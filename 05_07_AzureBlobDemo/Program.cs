#pragma warning disable CS8321 // Local function is declared but never used

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System.Text;

const string ConnectionString = "{INSERT AZURE STORAGE CONNECTION STRING}";

BlobServiceClient _client;
BlobContainerClient _container;

_client = GetClientViaConnectionString();
if (await CheckConnection())
{
    await InitializeContainer("sdk-test");
    await UploadBlobViaContainer("lorem.txt");
    //await UploadBlobViaClient("8k_image.jpg", "imageBlob.jpg");
    //await DownloadBlob("lorem.txt", "downloaded.txt");
    //await DeleteBlob("lorem.txt");
    //await ListContainerBlobs();
}

static BlobServiceClient GetClientViaConnectionString()
    => new(ConnectionString);

async Task<bool> CheckConnection()
{
    try
    {
        var accountInfo = await _client.GetAccountInfoAsync();
        Console.WriteLine($"Kind: {accountInfo.Value.AccountKind}");
        Console.WriteLine($"SKU:  {accountInfo.Value.SkuName}");
    }
    catch
    {
        Console.WriteLine("ERROR: Could not connect to Azure Storage Account.");
        return false;
    }

    return true;
}

Task InitializeContainer(string containerName)
{
    Console.WriteLine($"Initializing blob container '{containerName}'...");

    _container = _client.GetBlobContainerClient(containerName);
    return _container.CreateIfNotExistsAsync(PublicAccessType.None);
}

Task UploadBlobViaContainer(string blobName)
{
    byte[] blobContent = Encoding.ASCII.GetBytes("Lorem Ipsum Dolor Sit Amet");

    using var stream = new MemoryStream(blobContent);

    Console.WriteLine($"Uploading blob from stream to '{_container.Name}\\{blobName}'...");

    return _container.UploadBlobAsync(blobName, stream);
}

Task UploadBlobViaClient(string filePath, string blobName)
{
    BlobClient blob = _container.GetBlobClient(blobName);
    var options = new BlobUploadOptions()
    {
        AccessTier = AccessTier.Hot,
        HttpHeaders = new BlobHttpHeaders() { ContentType = "image/jpeg"},
        Metadata = new Dictionary<string, string>()
        {
            ["some_property"] = "some value"
        },
        Tags = new Dictionary<string, string>()
        {
            ["purpose"] = "demo"
        }
    };

    Console.WriteLine($"Uploading blob from {filePath} to '{_container.Name}\\{blobName}'...");

    return blob.UploadAsync(filePath, options);
}

Task DownloadBlob(string blobName, string destPath)
{
    BlobClient blob = _container.GetBlobClient(blobName);

    Console.WriteLine($"Downloading blob '{_container.Name}\\{blobName}' to {destPath}...");

    return blob.DownloadToAsync(destPath);
}

Task DeleteBlob(string blobName)
{
    BlobClient blob = _container.GetBlobClient(blobName);

    Console.WriteLine($"Deleting blob '{_container.Name}\\{blobName}'...");

    return blob.DeleteIfExistsAsync();
}

async Task ListContainerBlobs()
{
     await foreach (BlobItem blob in _container.GetBlobsAsync())
     {     
         Console.WriteLine($"{blob.Name}, {blob.Properties.ContentLength} B ({blob.Properties.AccessTier})");
     }
}

#pragma warning restore CS8321 // Local function is declared but never used
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
    await GetBlobProperties("imageBlob.jpg");
    await UpdateBlobProperties("imageBlob.jpg");
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

async Task GetBlobProperties(string blobName)
{
    BlobClient blob = _container.GetBlobClient(blobName);

    Console.WriteLine($"Retrieving properties of blob '{_container.Name}\\{blobName}'...");
    Console.WriteLine("");

    Response<BlobProperties> properties = await blob.GetPropertiesAsync();
    Console.WriteLine($"Access Tier:         {properties.Value.AccessTier}");
    Console.WriteLine($"Blob Type:           {properties.Value.BlobType}");
    Console.WriteLine($"Content Disposition: {properties.Value.ContentDisposition}");
    Console.WriteLine($"Size:                {properties.Value.ContentLength} B");
    Console.WriteLine("");

    Console.WriteLine("Metadata:");
    foreach (var metadata in properties.Value.Metadata)
    {
        Console.WriteLine($"{metadata.Key} = {metadata.Value}");
    }
    Console.WriteLine("");

    Console.WriteLine($"Retrieving indexing tags of blob '{_container.Name}\\{blobName}'...");
    Console.WriteLine("");

    GetBlobTagResult tags = await blob.GetTagsAsync();
    Console.WriteLine("Tags:");
    foreach (var tag in tags.Tags)
    {
        Console.WriteLine($"{tag.Key} = {tag.Value}");
    }
}

async Task UpdateBlobProperties(string blobName)
{
    await GetBlobProperties(blobName);

    BlobClient blob = _container.GetBlobClient(blobName);

    // Properties
    var properties = new BlobHttpHeaders()
    {
        ContentDisposition = "attachment; filename=\"8k_image.jpg\""
    };

    await blob.SetHttpHeadersAsync(properties);

    // Metadata
    var metadata = new Dictionary<string, string>()
    {
        ["meta1"] = "value 1",
        ["meta2"] = "value 2"
    };

    await blob.SetMetadataAsync(metadata);

    // Tags
    var tags = new Dictionary<string, string>()
    {
        ["tag1"] = "tag 1",
        ["tag2"] = "tag 2"
    };

    await blob.SetTagsAsync(tags);

    await GetBlobProperties(blobName);
}

#pragma warning restore CS8321 // Local function is declared but never used
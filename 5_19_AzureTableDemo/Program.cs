#pragma warning disable CS8321 // Local function is declared but never used

using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;

const string ConnectionString = "{INSERT AZURE STORAGE CONNECTION STRING}";

TableServiceClient _client;
TableClient _table;

_client = GetClientViaConnectionString();
if (await CheckConnection())
{
    //await ListAllTables();
    //await InitializeTable("students");
    //await ListAllTables();
    //await InsertStudent("Rokfort", "HG123", "Hermione Granger", 17, 1.00, true);
    //await InsertStudent("Rokfort", "HP007", "Harry Potter", 16, null, null);
    //await ListAllStudents();
    //await UpdateStudent("Rokfort", "HG123", null, 18, null, false);
    //await ListAllStudents();
    //await FilterStudentsByAge(0, 18);
    //await FilterStudentsByAge(17, 99);
    //await DeleteStudent("Rokfort", "HP007");
    //await ListAllStudents();
}

static TableServiceClient GetClientViaConnectionString()
    => new(ConnectionString);

async Task<bool> CheckConnection()
{
    try
    {
        var storageProperties = await _client.GetPropertiesAsync();
        Console.WriteLine($"Connected OK.");
        Console.WriteLine();
    }
    catch
    {
        Console.WriteLine("ERROR: Could not connect to Azure Storage Account.");
        return false;
    }

    return true;
}

async Task ListAllTables()
{
    Console.WriteLine("Tables:");

    await foreach (TableItem table in _client.QueryAsync())
    {     
        Console.WriteLine($"  {table.Name}");
    }
    
    Console.WriteLine();
}

Task InitializeTable(string tableName)
{
    Console.WriteLine($"Initializing table '{tableName}'...");
    Console.WriteLine();

    _table = _client.GetTableClient(tableName);
    return _table.CreateIfNotExistsAsync();
}

Task InsertStudent(
    string school,
    string studentCode,
    string? name,
    int? age,
    double? gradeAvg,
    bool? freeLunch)
{
    var student = new Student
    {
        SchoolName = school,
        StudentCode = studentCode,
        Name = name,
        Age = age,
        GradeAverage = gradeAvg,
        FreeLunch = freeLunch
    };

    Console.WriteLine($"Inserting new record to '{_table.Name}'...");
    Console.WriteLine();

    return _table.AddEntityAsync(student);
}

async Task ListAllStudents()
{
    Console.WriteLine("Students:");

    await foreach (Student student in _table.QueryAsync<Student>())
    {     
        Console.WriteLine($"  {student}");
    }
    
    Console.WriteLine();
}

Task UpdateStudent(
    string school,
    string studentCode,
    string? name,
    int? age,
    double? gradeAvg,
    bool? freeLunch)
{
    var student = new Student
    {
        SchoolName = school,
        StudentCode = studentCode,
        Name = name,
        Age = age,
        GradeAverage = gradeAvg,
        FreeLunch = freeLunch
    };

    Console.WriteLine($"Updating record in '{_table.Name}'...");
    Console.WriteLine("");

    return _table.UpdateEntityAsync(student, ETag.All, TableUpdateMode.Replace);
}

async Task FilterStudentsByAge(int minAge, int maxAge)
{
    Console.WriteLine($"Showing students with ages {minAge} - {maxAge}");

    await foreach (Student student in _table.QueryAsync<Student>(
        student => student.Age >= minAge && student.Age <= maxAge,
        null,
        ["PartitionKey, RowKey"]))
    {     
        Console.WriteLine(student.ToString());
    }
    
    Console.WriteLine();
}

Task DeleteStudent(string partitionKey, string rowKey)
{
    Console.WriteLine($"Deleting record from '{_table.Name}'...");

    return _table.DeleteEntityAsync(partitionKey, rowKey);
}

#pragma warning restore CS8321 // Local function is declared but never used
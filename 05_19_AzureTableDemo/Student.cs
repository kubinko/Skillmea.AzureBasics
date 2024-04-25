using System.Runtime.Serialization;
using Azure;
using Azure.Data.Tables;

class Student : ITableEntity
{
    // Record properties
    [IgnoreDataMember]
    public string SchoolName { get => PartitionKey; set => PartitionKey = value; }

    [IgnoreDataMember]
    public string StudentCode { get => RowKey; set => RowKey = value; }

    public string? Name { get; set; }
    public int? Age { get; set; }
    public double? GradeAverage { get; set; }
    public bool? FreeLunch { get; set; }

    // ITableEntity implementation
    public string PartitionKey { get; set; } = "";
    public string RowKey { get; set; } = "";
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public override string ToString()
        => string.Concat(
            $"{SchoolName}; ",
            $"{StudentCode}",
            !string.IsNullOrEmpty(Name) ? $" {Name}" : string.Empty,
            Age != null ? $"; Age: {Age}" : string.Empty,
            GradeAverage != null ? $"; Avg: {GradeAverage:0.00}" : string.Empty,
            FreeLunch != null ? $"; Lunch: {FreeLunch}" : string.Empty);
}
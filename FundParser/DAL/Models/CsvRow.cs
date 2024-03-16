using CsvHelper.Configuration.Attributes;

namespace DAL.Models;

public class CsvRow
{
    [Index(0)]
    public string date { get; set; }
    
    [Index(1)]
    public string fund { get; set; }
    
    [Index(2)]
    public string company { get; set; }
    
    [Index(3)]
    public string ticker { get; set; }
    
    [Index(4)]
    public string cusip { get; set; }
    
    [Index(5)]
    public string shares { get; set; }
    
    [Index(6)]
    public string marketValue { get; set; }
    
    [Index(7)]
    public string weight { get; set; }
}
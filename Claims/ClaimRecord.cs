using System;

namespace Claims;

public class ClaimRecord
{
    // properties of the class
    public required string Product {get; set;}
    public required int OriginYear {get; set;}
    public required int DevelopmentYear {get; set;}
    public required double IncrementalValue {get; set;}

}

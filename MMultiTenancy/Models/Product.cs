 
public class Product: IMustHaveTanent
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Rate { get; set; }
    public string TenatId { get; set; } = null!; 
}
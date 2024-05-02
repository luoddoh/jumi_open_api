namespace Admin.NET.Application;
public class SaveSetUpInput
{
    public long switchId { get; set; }
    public List<long>? children { get; set; }
}
public class SaveSystemInput
{
    public long Id { get; set; }
    public string Value {  get; set; }
}

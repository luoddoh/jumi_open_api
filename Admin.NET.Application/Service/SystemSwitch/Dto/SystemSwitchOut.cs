namespace Admin.NET.Application;
public class SystemSwitchOut
{
    public long switchId { get; set; }
    public string switchName { get; set; }
    public string switchValue { get; set; }

    public List<User>? children { get; set; }
}
public class User
{
    public long UserId { get; set; }
    public string UserName { get; set; }
}

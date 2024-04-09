namespace Admin.NET.Application;

public class MiniAppLoginInput
{
    /// <summary>
    /// 账号
    /// </summary>
    /// <example>admin</example>
    [Required(ErrorMessage = "账号不能为空"), MinLength(2, ErrorMessage = "账号不能少于2个字符")]
    public string Account { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    /// <example>123456</example>
    [Required(ErrorMessage = "密码不能为空"), MinLength(3, ErrorMessage = "密码不能少于3个字符")]
    public string Password { get; set; }
}

﻿// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

namespace Admin.NET.Core.Service;

public class CreateEntityInput
{
    /// <summary>
    /// 表名
    /// </summary>
    /// <example>student</example>
    public string TableName { get; set; }

    /// <summary>
    /// 实体名
    /// </summary>
    /// <example>Student</example>
    public string EntityName { get; set; }

    /// <summary>
    /// 基类名
    /// </summary>
    /// <example>AutoIncrementEntity</example>
    public string BaseClassName { get; set; }

    /// <summary>
    /// 导出位置
    /// </summary>
    /// <example>Web.Application</example>
    public string Position { get; set; }

    /// <summary>
    /// 库标识
    /// </summary>
    public string ConfigId { get; set; }
}
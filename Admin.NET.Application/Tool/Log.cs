// 大名科技（天津）有限公司版权所有  电话：18020030720  QQ：515096995
//
// 此源代码遵循位于源代码树根目录中的 LICENSE 文件的许可证

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Tool;
public class Log
{
    public static void error(string message)
    {
        try
        {
            string logFilePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + $"/Log/log_{DateTime.Now.ToString("yyyy-MM-dd")}.text";
            // 确保日志文件目录存在
            var logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // 写入日志
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:【{message}】\n");
            }
        }
        catch (Exception ex)
        {
            // 异常处理
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}

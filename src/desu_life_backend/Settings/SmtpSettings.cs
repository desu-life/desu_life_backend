namespace desu.life.Settings;

// 先创建了个类 EmailSettings 用于存储邮箱相关的配置信息
public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
    public string Sender { get; set; }
}
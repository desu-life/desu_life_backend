using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using desu.life.Services.CDKeyservice;
using System.Text;

namespace desu.life.Controllers;

// roles = [ "System", "Bot", "Administrator", "Moderator", "CoOrganizer", "PremiumUser", "User" ];

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("cdkeytest")]
    public string Get()
    {
        // 生成CDKey, 有效期1年, 类型1, 编号1001
        var cdkey = CDKeyservice.GenerateCDKey("4", DateTime.Now.AddYears(1));

        // 解密CDKey
        var unscrambledCdKey = CDKeyservice.UnscrambleCDKey(cdkey);

        // 解码信息
        var decodedInfo = Encoding.UTF8.GetString(CDKeyservice.PrivateBase32Decode(unscrambledCdKey[..20]));

        // 验证CDKey
        //var isValid = CDKeyservice.VerifyCDKey(cdkey, decodedInfo);
        var isValid = false;

        // base32
        var base32 = CDKeyservice.PrivateBase32Encode(Encoding.UTF8.GetBytes(decodedInfo));

        return $"Hello! Your CDKey is {cdkey}, unscrambledCdKey is {unscrambledCdKey}, decodedInfo is {decodedInfo}, Base32 is {base32}, This key is {(isValid ? "vaild.":"invaild")}";
    }
}
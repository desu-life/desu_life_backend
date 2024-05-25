#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace desu.life.Data.Models;

public class RedeemCode
{
    public string Issuer { get; set; }

    public string CodeType { get; set; } // 兑换码类型 比如促销码、礼包码、邀请码、激活码等

    public string Code { get; set; } // 兑换码

    public string Item { get; set; } // 兑换码对应的物品，仅在兑换码类型需要兑换物品时有用

    public bool AllowMultipleRedemptions { get; set; } // 是否允许多次兑换

    public DateTime CreationTime { get; set; } // 兑换码的创建时间

    public DateTime ExpirationTime { get; set; } // 兑换码的过期时间

    public DateTime ItemExpiration { get; set; } // 所对换后物品过期时间

    public string RedeemedBy { get; set; } // 谁兑换了

    public bool RedemptionStatus { get; set; } // 是否已兑换

    public int RedemptionLimit { get; set; } // 次数限制

    public int RedemptionCount { get; set; } // 已兑换次数
}
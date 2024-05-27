using desu.life.Data;
using desu.life.Data.Models;
using Microsoft.EntityFrameworkCore;

public class RedeemCodeService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public RedeemCodeService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<bool> InsertOrUpdateRedeemCode(string code, string codeType, string item, DateTime creationTime, DateTime expirationTime, bool allowMultipleRedemptions, DateTime itemExpiration, string redeemedBy, bool redemptionStatus, string issuer, int redemptionLimit)
    {
        var existingCode = await _applicationDbContext.RedeemCodes.FirstOrDefaultAsync(c => c.Code == code);

        if (existingCode == null)
        {
            // 如果兑换码不存在，则创建新的兑换码
            var newRedeemCode = new RedeemCode
            {
                Code = code,
                CodeType = codeType,
                Item = item,
                CreationTime = creationTime,
                ExpirationTime = expirationTime,
                AllowMultipleRedemptions = allowMultipleRedemptions,
                ItemExpiration = itemExpiration,
                RedeemedBy = redeemedBy,
                RedemptionStatus = redemptionStatus,
                Issuer = issuer,
                RedemptionLimit = redemptionLimit
            };
            _applicationDbContext.RedeemCodes.Add(newRedeemCode);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        // 如果兑换码已存在
        return false;
    }
}

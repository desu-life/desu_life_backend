using System;
using System.Text;
using System.Security.Cryptography;

namespace desu.life.Services.CDKeyservice;

public class CDKeyservice
{
    // private static readonly int[] scrambleIndex = [2, 14, 8, 19, 6, 24, 0, 23, 4, 21, 16, 18, 1, 13, 11, 22, 7, 3, 10, 15, 5, 17, 9, 12, 20];
    private static readonly int[] scrambleIndex = [3, 24, 23, 19, 2, 11, 13, 17, 5, 20, 16, 6, 7, 22, 18, 1, 10, 14, 15, 0, 4, 21, 9, 8, 12];
    // private static readonly int[] scrambleIndex = [18, 7, 20, 14, 21, 6, 12, 23, 2, 19, 5, 3, 0, 1, 16, 9, 17, 22, 15, 4, 13, 10, 11, 24, 8];
    // private static readonly int[] scrambleIndex = [18, 16, 15, 17, 7, 12, 14, 19, 24, 11, 8, 21, 23, 1, 0, 9, 20, 3, 6, 22, 2, 5, 4, 10, 13];
    private static readonly char[] privateBase32table = "123456789ABCDEFGHJKLMNPRSTVWXY".ToCharArray();
    private static readonly Dictionary<char, int> indexTable = CreateIndexTable();

    // redeemInfo: num
    // 例：1表示兑换类型1
    // 有些复杂 不确定对安全性有多大影响，但是可以增加破解的难度，写着玩的。这个可以不用插入到数据库，只要验证通过就可以了，但是不存数据库的话总感觉有点不好

    public static string GenerateCDKey(string redeemInfo, DateTime expiryDate)
    {
        // 兑换信息 + 过期日期 + 序列号
        byte[] randomNumber = new byte[4];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        int sequenceNumber = BitConverter.ToInt32(randomNumber, 0);

        // 合并信息
        string combinedInfo = redeemInfo + expiryDate.ToString("yyMM") + sequenceNumber;

        // 编码信息
        string encodedInfo = PrivateBase32Encode(Encoding.UTF8.GetBytes(combinedInfo));

        // 补全到20位
        if (encodedInfo.Length < 20)
        {
            encodedInfo = encodedInfo.PadRight(20, 'Z');
        }

        // 生成SHA256哈希
        string hash = GenerateSHA256Hash(encodedInfo);

        // 选择哈希的部分字符
        string hashPart = GetSafeHashCharacters(hash, [1, 6, 11, 16, 20]);

        return ScrambleCDKey(encodedInfo + hashPart);
    }

    // 验证CDKey
    public static bool VerifyCDKey(string cdKey, string redeemInfo)
    {
        // 解密CDKey
        string unscrambledCdKey = UnscrambleCDKey(cdKey);

        // 解码信息
        string decodedInfo = Encoding.UTF8.GetString(PrivateBase32Decode(unscrambledCdKey.Substring(0, 20)));

        // 解析信息
        string decodedRedeemInfo = decodedInfo[..redeemInfo.Length];
        string decodedExpiryDate = decodedInfo.Substring(redeemInfo.Length, 4);
        string decodedSequenceNumber = decodedInfo.Substring(redeemInfo.Length + 4, 4);

        // 验证信息
        if (decodedRedeemInfo != redeemInfo)
        {
            return false;
        }

        // 验证过期日期，只验证年月，如果年月超过了过期日期，就认为无效。
        if (DateTime.Now > DateTime.Parse(decodedExpiryDate))
        {
            return false;
        }

        // 验证哈希
        string hash = GenerateSHA256Hash(decodedInfo);
        string hashPart = GetSafeHashCharacters(hash, [1, 6, 11, 16, 20]);

        return hashPart == unscrambledCdKey[20..];
    }

    private static string GenerateSHA256Hash(string input)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }

    public static string PrivateBase32Encode(byte[] bytes)
    {
        StringBuilder result = new((int)(bytes.Length * 8 / 5.0 + 1));
        int bitsCollected = 0;
        int accumulator = 0;

        foreach (byte b in bytes)
        {
            accumulator = (accumulator << 8) | b;
            bitsCollected += 8;

            while (bitsCollected >= 5)
            {
                bitsCollected -= 5;
                int currentBit = (accumulator >> bitsCollected) & 0x1F;
                result.Append(privateBase32table[currentBit]);
            }
        }

        // Process remaining bits
        if (bitsCollected > 0)
        {
            accumulator <<= (5 - bitsCollected);
            int currentBit = (accumulator & 0x1F);
            result.Append(privateBase32table[currentBit]);
        }

        // while ((result.Length % 8) != 0)
        // {
        //      result.Append('Z');  // Use 'Z' as the padding character
        // }

        return result.ToString();
    }

    public static byte[] PrivateBase32Decode(string encoded)
    {
        List<byte> output = new();
        int bitsCollected = 0;
        int accumulator = 0;

        foreach (char c in encoded)
        {
            if (c == 'Z') break;
            if (!indexTable.TryGetValue(c, out int value))
                throw new ArgumentException("Invalid character in the encoded string.");

            accumulator = (accumulator << 5) | value;
            bitsCollected += 5;

            if (bitsCollected >= 8)
            {
                bitsCollected -= 8;
                output.Add((byte)((accumulator >> bitsCollected) & 0xFF));
            }
        }

        return [.. output];
    }

    private static string GetSafeHashCharacters(string hash, int[] positions)
    {
        StringBuilder builder = new();
        foreach (int position in positions)
        {
            builder.Append(hash[position]);
        }
        return builder.ToString();
    }

    public static string ScrambleCDKey(string cdKey)
    {
        char[] scrambled = new char[25];
        for (int i = 0; i < scrambleIndex.Length; i++)
        {
            scrambled[scrambleIndex[i]] = cdKey[i];
        }

        return new string(scrambled);
    }

    public static string UnscrambleCDKey(string scrambledCdKey)
    {
        char[] unscrambled = new char[25];
        for (int i = 0; i < 25; i++)
        {
            unscrambled[i] = scrambledCdKey[scrambleIndex[i]];
        }

        return new string(unscrambled);
    }

    private static Dictionary<char, int> CreateIndexTable()
    {
        var index = new Dictionary<char, int>();
        for (int i = 0; i < privateBase32table.Length; i++)
        {
            index[privateBase32table[i]] = i;
        }
        return index;
    }
}
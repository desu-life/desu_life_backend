namespace desu.life.Utils;

using System.Text;

public static class RedeemCodeGenerator
{
    private static readonly string CDKeyChars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

    public static string Generate()
    {
        var random = new Random();
        var cdKey = new StringBuilder();
        for (int i = 1; i <= 25; i++)
        {
            cdKey.Append(CDKeyChars[random.Next(CDKeyChars.Length)]);
            if (i % 5 == 0 && i != 25)
            {
                cdKey.Append('-');
            }
        }

        return cdKey.ToString();
    }
}

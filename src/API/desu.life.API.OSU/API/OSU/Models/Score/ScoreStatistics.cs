#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class ScoreStatistics
{
    [JsonPropertyName("count_100")]
    public uint Count100 { get; set; }

    [JsonPropertyName("count_300")]
    public uint Count300 { get; set; }

    [JsonPropertyName("count_50")]
    public uint Count50 { get; set; }

    [JsonPropertyName("count_geki")]
    public uint? CountGeki { get; set; }

    [JsonPropertyName("count_katu")]
    public uint? CountKatu { get; set; }

    [JsonPropertyName("count_miss")]
    public uint CountMiss { get; set; }

    public uint TotalHits(GameMode mode)
    {
        var amount = Count300 + Count100 + CountMiss;
        if (mode is not GameMode.Taiko)
        {
            amount += Count50;

            if (mode is not GameMode.Osu)
            {
                amount += CountKatu ?? 0;
                amount += mode is not GameMode.Fruits ? CountGeki ?? 0 : 0;
            }
        }
        return amount;
    }

    public double Accuracy(GameMode mode)
    {
        var amount = (double)TotalHits(mode);

        var (numerator, denumerator) = mode switch
        {
            GameMode.Taiko => (0.5 * Count100 + Count300, amount),
            GameMode.Fruits => (Count50 + Count100 + Count300, amount),
            GameMode.Osu => (Count50 * 50 + Count100 * 100 + Count300 * 300, amount * 300.0),
            GameMode.Mania => (Count50 * 50 + Count100 * 100 + Count300 * 300 + CountKatu.Value * 200 + CountGeki.Value * 300, amount * 300.0),
            _ => throw new NotImplementedException(),
        };

        return double.Round(10_000.0 * numerator / denumerator) / 100.0;
    }
}

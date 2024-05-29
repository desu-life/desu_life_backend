using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public enum GameMode {
    /// osu!standard
    Osu = 0,
    /// osu!taiko
    Taiko = 1,
    /// osu!catch
    Fruits = 2,
    /// osu!mania
    Mania = 3,
}
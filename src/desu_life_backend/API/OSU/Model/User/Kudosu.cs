﻿#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;

namespace desu.life.API;

public partial class OSU
{
    public partial class Models
    {
        public class Kudosu
        {
            [JsonPropertyName("available")]
            public int Available { get; set; }

            [JsonPropertyName("total")]
            public int Total { get; set; }
        }
    }
}
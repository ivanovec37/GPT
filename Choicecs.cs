﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GPT
{
    public partial class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("message")]
        public ChatGptResponse Message { get; set; } = new();
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = "";
    }
}

﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNM.Twitter.Application.DTO
{
    public class PostTweetDto
    {
        [JsonProperty("text")]
        public string Text { get; set; }=string.Empty;
    }
}

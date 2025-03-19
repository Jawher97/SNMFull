using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace SNM.Instagram.Domain.Entities
{
    public class Insight
    {
        public string Id { get; set; }
        public int Comments { get; set; }
        public int Shares { get; set; }
        public string name { get; set; } 
        public string period { get; set; }
        public string title { get; set; }
        public string follows { get; set; }
        public string likes { get; set; } 
        public string profile_visits { get; set; }
    }
    
}


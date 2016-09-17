﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousEvents
{
    public class ShieldsUpEvent : Event
    {
        public const string NAME = "Shields up";
        public const string DESCRIPTION = "Triggered when your ship's shields come online";
        public static ShieldsUpEvent SAMPLE = new ShieldsUpEvent(DateTime.Now);
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static ShieldsUpEvent()
        {
            SAMPLE.raw = "{\"timestamp\":\"2016-07-22T10:53:19Z\",\"event\":\"ShieldState\",\"ShieldsUp\":true}";
        }

        public ShieldsUpEvent(DateTime timestamp) : base(timestamp, NAME)
        {
        }
    }
}

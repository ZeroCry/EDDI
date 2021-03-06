﻿using EddiDataDefinitions;
using EddiEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace EddiMissionMonitor
{
    public class MissionsEvent : Event
    {
        public const string NAME = "Missions";
        public const string DESCRIPTION = "Triggered at startup, with basic information of the Mission Log";
        public const string SAMPLE = "{ \"timestamp\":\"2017-10-02T10:37:58Z\", \"event\":\"Missions\", \"Active\":[ { \"MissionID\":65380900, \"Name\":\"Mission_Courier_name\", \"PassengerMission\":false, \"Expires\":82751 } ], \"Failed\":[  ], \"Complete\":[  ]}";
        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static MissionsEvent()
        {
            VARIABLES.Add("missions", "missions in the mission log (this is a list of Mission objects)");
        }

        [JsonProperty("missions")]
        public List<Mission> missions { get; private set; }

       public MissionsEvent(DateTime timestamp, List<Mission> missions) : base(timestamp, NAME)
        {
            this.missions = missions;
        }
    }
}

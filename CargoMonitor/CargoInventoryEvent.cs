using EddiDataDefinitions;
using EddiEvents;
using System;
using System.Collections.Generic;

namespace EddiCargoMonitor
{
    public class CargoInventoryEvent : Event
    {
        public const string NAME = "Cargo inventory";
        public const string DESCRIPTION = "Triggered when you obtain an inventory of your cargo";
        public const string SAMPLE = "{ \"timestamp\":\"2018-10-31T03:39:10Z\", \"event\":\"Cargo\", \"Count\":32, \"Inventory\":[ { \"Name\":\"hydrogenfuel\", \"Name_Localised\":\"Hydrogen Fuel\", \"Count\":1, \"Stolen\":0 }, { \"Name\":\"biowaste\", \"MissionID\":426282789, \"Count\":30, \"Stolen\":0 }, { \"Name\":\"animalmeat\", \"Name_Localised\":\"Animal Meat\", \"Count\":1, \"Stolen\":0 } ] }";

        public static Dictionary<string, string> VARIABLES = new Dictionary<string, string>();

        static CargoInventoryEvent()
        {
            VARIABLES.Add("inventory", "The cargo in your inventory");
            VARIABLES.Add("cargocarried", "The total amount of cargo in your inventory");
        }

        public List<CargoInfo> inventory { get; private set; }
        public int cargocarried { get; private set; }

        public CargoInventoryEvent(DateTime timestamp, List<CargoInfo> inventory, int cargocarried) : base(timestamp, NAME)
        {
            this.inventory = inventory;
            this.cargocarried = cargocarried;
        }
    }
}

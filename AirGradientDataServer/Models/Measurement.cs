using System;

namespace AirGradientDataServer.Models
{
    public class Measurement
    {
        public long Id { get; set; }
        public DateTime MeasurementTime { get; set; }
        public string? DeviceId { get; set; }
        public int? WifiStrength { get; set; } = null;
        public int? PM25 { get; set; } = null;
        public int? CO2 { get; set; } = null;
        public float? Temperature { get; set; } = null;
        public int? Humidity { get; set; } = null;
    }
}

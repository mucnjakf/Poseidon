﻿namespace Poseidon.DAL.Entities;

public class LatestEventEntity
{
    public int Id { get; set; }

    public string Mmsi { get; set; }

    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public DateTime StartTimestamp { get; set; }

    public DateTime EndTimestamp { get; set; }

    public double MedianSpeedKnots { get; set; }

    public double TotalDurationHours { get; set; }

    public bool IsProcessed { get; set; }
}
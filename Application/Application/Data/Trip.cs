using System;
using System.Collections.Generic;

namespace Application.Data;

public partial class Trip
{
    public int Id { get; set; }

    public DateOnly? TpepPickupDatetime { get; set; }

    public DateOnly? TpepDropoffDatetime { get; set; }

    public int? PassengerCount { get; set; }

    public double? TripDistance { get; set; }

    public string? StoreAndFwdFlag { get; set; }

    public int? PulocationId { get; set; }

    public int? DolocationId { get; set; }

    public decimal? FareAmount { get; set; }

    public decimal? TipAmount { get; set; }
}

namespace MMEmergencyCall.Domain.Admin.Features.Townships;

public class TownshipModel
{
    public int TownshipId { get; set; }

    public string TownshipCode { get; set; } = null!;

    public string TownshipNameEn { get; set; } = null!;

    public string TownshipNameMm { get; set; } = null!;

    public string StateRegionCode { get; set; } = null!;
}

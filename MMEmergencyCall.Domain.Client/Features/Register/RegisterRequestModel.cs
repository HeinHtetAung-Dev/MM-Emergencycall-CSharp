namespace MMEmergencyCall.Domain.Client.Features.Register;

public class RegisterRequestModel
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string TownshipCode { get; set; } = null!;
    public string? EmergencyType { get; set; }
    public string? EmergencyDetails { get; set; }
}

public class VerifyRequestModel
{
    public string Email { get; set; } = null!;
    public string OTP { get; set; } = null!;
}


public enum EnumVerify
{
    Y,
    N,
}
public enum EnumUserStatus
{
    Approved,
    Pending,
}

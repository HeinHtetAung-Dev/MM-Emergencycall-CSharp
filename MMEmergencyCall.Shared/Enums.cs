namespace MMEmergencyCall.Shared
{
    #region EmergencyRequest

    public enum EnumEmergencyRequestStatus
    {
        Cancel,
        Open,
        Closed
    }

    #endregion

    #region User

    public enum EnumUserStatus
    {
        Pending,
        Deactivate,
    }

    #endregion

    #region Emergency Service
    public enum EnumServiceStatus
    {
        None,
        Pending,
        Approved,
        Rejected
    }
    #endregion
}

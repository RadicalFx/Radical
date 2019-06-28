namespace Radical.Win32
{
#pragma warning disable 1591

    public enum WindowLong : int
    {
        WindowProc /* */ = -4,
        HInstance /*  */ = -6,
        HWndParent /* */ = -8,
        Style /*      */ = Constants.GWL_STYLE,
        ExStyle /*    */ = -20,
        UserData /*   */ = -21,
        ID /*         */ = -12,
    }

#pragma warning restore 1591

}

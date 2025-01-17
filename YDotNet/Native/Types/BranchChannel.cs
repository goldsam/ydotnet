using System.Runtime.InteropServices;

namespace YDotNet.Native.Types;

internal static class BranchChannel
{
    public delegate void ObserveCallback(nint state, uint length, nint eventsHandle);

    [DllImport(ChannelSettings.NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yobserve_deep")]
    public static extern uint ObserveDeep(nint type, nint state, ObserveCallback callback);

    [DllImport(ChannelSettings.NativeLib, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yunobserve_deep")]
    public static extern uint UnobserveDeep(nint type, uint subscriptionId);
}

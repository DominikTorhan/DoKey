using System;
using System.Runtime.InteropServices;

namespace DoKey.App
{
  [StructLayout(LayoutKind.Sequential)]
  public struct LowLevelKeyboardInputEvent
  {
    public int VirtualCode;
    public int HardwareScanCode;
    public int Flags;
    public int TimeStamp;
    public IntPtr AdditionalInformation;
  }
}
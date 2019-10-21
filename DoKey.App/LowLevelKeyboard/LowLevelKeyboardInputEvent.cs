using System.Runtime.InteropServices;

namespace DoKey.App.LowLevelKeyboard
{
  [StructLayout(LayoutKind.Sequential)]
  public struct LowLevelKeyboardInputEvent
  {
    public int VirtualCode;
    public int HardwareScanCode;
    public int Flags;
    public int TimeStamp;
  }
}
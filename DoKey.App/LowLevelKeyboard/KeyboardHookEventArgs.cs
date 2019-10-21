using System.ComponentModel;

namespace DoKey.App.LowLevelKeyboard
{
  class KeyboardHookEventArgs : HandledEventArgs
  {
    public readonly KeyboardState KeyboardState;
    public readonly LowLevelKeyboardInputEvent KeyboardData;

    public KeyboardHookEventArgs(LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState)
    {
      KeyboardData = keyboardData;
      KeyboardState = keyboardState;
    }
  }
}
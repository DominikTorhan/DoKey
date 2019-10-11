using System.ComponentModel;

namespace DoKey.App
{
  class KeyboardHookEventArgs : HandledEventArgs
  {
    public KeyboardState KeyboardState { get; private set; }
    public LowLevelKeyboardInputEvent KeyboardData { get; private set; }

    public KeyboardHookEventArgs(LowLevelKeyboardInputEvent keyboardData, KeyboardState keyboardState)
    {
      KeyboardData = keyboardData;
      KeyboardState = keyboardState;
    }
  }
}
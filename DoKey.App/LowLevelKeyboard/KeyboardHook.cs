using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DoKey.App.LowLevelKeyboard
{

  public enum KeyboardState
  {
    KeyDown = 256,
    KeyUp = 257,
    SysKeyDown = 260,
    SysKeyUp = 261
  }

  //Based on https://gist.github.com/Stasonix
  class KeyboardHook : IDisposable
  {

    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("USER32", SetLastError = true)]
    static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

    [DllImport("USER32", SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hHook);

    [DllImport("USER32", SetLastError = true)]
    static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

    private IntPtr _windowsHookHandle;
    private IntPtr _user32LibraryHandle;
    private HookProc _hookProc;

    private event EventHandler<KeyboardHookEventArgs> KeyboardPressed;
    delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    public const int WH_KEYBOARD_LL = 13;
    public const int VkSnapshot = 0x2c;
    const int KfAltdown = 0x2000;
    public const int LlkhfAltdown = (KfAltdown >> 8);

    ~KeyboardHook()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public KeyboardHook(EventHandler<KeyboardHookEventArgs> eventHandler)
    {
      _windowsHookHandle = IntPtr.Zero;
      _user32LibraryHandle = IntPtr.Zero;
      _hookProc = LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

      _user32LibraryHandle = LoadLibrary("User32");
      if (_user32LibraryHandle == IntPtr.Zero)
      {
        int errorCode = Marshal.GetLastWin32Error();
        throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
      }

      _windowsHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _hookProc, _user32LibraryHandle, 0);
      if (_windowsHookHandle == IntPtr.Zero)
      {
        int errorCode = Marshal.GetLastWin32Error();
        throw new Win32Exception(errorCode, $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
      }

      KeyboardPressed += eventHandler;

    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        // because we can unhook only in the same thread, not in garbage collector thread
        if (_windowsHookHandle != IntPtr.Zero)
        {
          if (!UnhookWindowsHookEx(_windowsHookHandle))
          {
            int errorCode = Marshal.GetLastWin32Error();
            throw new Win32Exception(errorCode, $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
          }
          _windowsHookHandle = IntPtr.Zero;

          // ReSharper disable once DelegateSubtraction
          _hookProc -= LowLevelKeyboardProc;
        }
      }

      if (_user32LibraryHandle != IntPtr.Zero)
      {
        if (!FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
        {
          int errorCode = Marshal.GetLastWin32Error();
          throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
        }
        _user32LibraryHandle = IntPtr.Zero;
      }
    }

    private IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
    {

      bool fEatKeyStroke = false;

      var wparamTyped = wParam.ToInt32();
      if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
      {
        object o = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
        LowLevelKeyboardInputEvent p = (LowLevelKeyboardInputEvent)o;

        var eventArguments = new KeyboardHookEventArgs(p, (KeyboardState)wparamTyped);

        InvokeKeyboardPressed(eventArguments);

        fEatKeyStroke = eventArguments.Handled;
      }

      return fEatKeyStroke ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }

    private void InvokeKeyboardPressed(KeyboardHookEventArgs eventArgs)
    {
      KeyboardPressed?.Invoke(this, eventArgs);
    }

  }
}
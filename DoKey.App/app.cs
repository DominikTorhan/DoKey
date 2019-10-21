using DoKey.App.LowLevelKeyboard;
using DoKey.CoreCS;
using System.Windows.Forms;

namespace DoKey.App
{
  internal class App
  {
    private readonly KeyboardHook keyboardHook;
    private bool isSending;
    private Session session;
    private readonly AppConfig appConfig;

    public App(AppConfig appConfig)
    {
      isSending = false;
      this.appConfig = appConfig;
      session = DomainUtils.CreateSession(appConfig.funcGetConfigText);
      keyboardHook = CreateKeyboardHook();
    }

    private KeyboardHook CreateKeyboardHook()
    {
      return new KeyboardHook(OnKeyPressed);
    }

    private void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
      if (isSending) return;
      if (IsSystemCapsLock(e)) return;
      isSending = true;
      var keyEvent = CreateKeyEvent(e);
      var output = new AppProcessor(appConfig, session, keyEvent).PerformKeyPress();
      isSending = false;
      if (output == null) return;
      e.Handled = output.preventKeyProcess;
      session = new Session(session.config, output.appState);
    }

    private KeyEvent CreateKeyEvent(KeyboardHookEventArgs e)
    {
      var key = ((Keys)e.KeyboardData.VirtualCode).ToString();
      var isUp = false;
      if (e.KeyboardState == KeyboardState.KeyUp) isUp = true;
      if (e.KeyboardState == KeyboardState.SysKeyUp) isUp = true;
      return new KeyEvent(key, isUp);
    }

    private bool IsSystemCapsLock(KeyboardHookEventArgs e)
    {
      if (!Control.IsKeyLocked(Keys.CapsLock)) return false;
      var key = (Keys)e.KeyboardData.VirtualCode;
      if (key != Keys.Capital) return false;
      return true;
    }

    public void DisposeKeyboardHook()
    {
      keyboardHook?.Dispose();
    }

  }
}

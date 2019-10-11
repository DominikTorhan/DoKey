using DoKey.CoreCS;
using DoKey.CoreCS.KeysProcessor;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DoKey.App
{

  class App
  {
    private Session session;
    private bool isSending;
    private readonly Action<State> actionRefreshIcon;
    private readonly KeyboardHook keyboardHook;
    private readonly Action actionExit;
    private readonly Logger logger;

    public App(Action<State> actionRefreshIcon, Action actionExit, Logger logger)
    {
      isSending = false;
      session = DomainUtils.CreateSession(() => File.ReadAllText(DomainUtils.filePathNew));
      this.keyboardHook = CreateKeyboardHook();
      this.actionRefreshIcon = actionRefreshIcon;
      this.actionExit = actionExit;
      this.logger = logger;
    }

    private KeyboardHook CreateKeyboardHook()
    {
      var keyboardHook = new KeyboardHook();
      keyboardHook.KeyboardPressed += OnKeyPressed;
      return keyboardHook;
    }

    private KeyEventData CreateKetEventData(KeyboardHookEventArgs e)
    {
      KeyEventType keyEventType = KeyEventType.Down;
      if (e.KeyboardState == KeyboardState.KeyUp) keyEventType = KeyEventType.Up;
      if (e.KeyboardState == KeyboardState.SysKeyUp) keyEventType = KeyEventType.Up;
      var key = (Keys)e.KeyboardData.VirtualCode;
      var inputKey = DomainUtils.CreateInputKey(key.ToString());
      return new KeyEventData { inputKey = inputKey, keyEventType = keyEventType };
    }

    private void OnKeyPressed(object sender, KeyboardHookEventArgs e)
    {
      OnKeyPressed(e);
    }

    private void OnKeyPressed(KeyboardHookEventArgs e)
    {
      if (isSending) return;

      Keys key = (Keys)e.KeyboardData.VirtualCode;
      if (Control.IsKeyLocked(Keys.CapsLock) && key == Keys.Capital) return;

      KeyEventData keyEventData = CreateKetEventData(e);

      var output = Work(keyEventData);
      if (output == null) return;

      if (output.preventKeyProcess) e.Handled = true;
      if (output.send == "")
      {
        actionRefreshIcon(session.appState.state);
        return;
      }
      if (TryRunCommandKey(output.send)) return;

      isSending = true;
      logger.Log($"{keyEventData} send: {output.send}");
      SendKeys.Send(output.send);
      isSending = false;

    }

    private bool TryRunCommandKey(string send)
    {
      if (send == "#exit") { actionExit(); return true; }
      if (send == "#config") { OpenSettings(); return true; }
      return false;

    }

    private KeysEngineResult Work(KeyEventData keyEventData)
    {
      var output = ProcessKey(keyEventData);
      if (output == null) return null;
      session = new Session { config = session.config, appState = output.appState };
      return output;
    }

    private KeysEngineResult ProcessKey(KeyEventData keyEventData)
    {
      bool isUp = keyEventData.keyEventType == KeyEventType.Up;

      var processor = new KeysProcessor(session.appState,
        keyEventData.inputKey,
        isUp,
        session.config);

      return processor.ProcessKey();

    }

    //private bool TryExitApp(Keys key, AppState appState)
    //{
    //  if (!appState.modificators.caps) return false;
    //  if (key != Keys.Back) return false;
    //  actionExit();
    //  return true;
    //}

    //private bool TryOpenSettingsFile(Keys key, AppState appState)
    //{
    //  if (!appState.modificators.caps) return false;
    //  if (key != Keys.Oem2) return false;
    //  OpenSettings();
    //  return true;

    //}

    private void OpenSettings()
    {
      Process.Start(DomainUtils.filePathNew);
    }

    internal void Dispose()
    {
      keyboardHook?.Dispose();
    }

  }

}

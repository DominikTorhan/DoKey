using DoKey.CoreCS;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DoKey.App
{
  //class App
  //{
  //  private Session _session;

  //  public void Initialize(Func<string> GetConfigText)
  //  {

  //    _session = DomainOperations.CreateSession;

  //  }

  //  public KeysEngineResult Work(KeyEventData keyEventData)
  //  {

  //    KeysEngineResult output = ProcessKey(keyEventData);

  //    if (output == null) return null;

  //    _session = new Session(_session.config, output.appState);

  //    return output;

  //  }

  //  private KeysEngineResult ProcessKey(KeyEventData keyEventData)
  //  {
  //    bool isUp = keyEventData.keyEventType == KeyEventType.Up;

  //    return new KeysEngine
  //    {
  //      AppStateX = _session.appState,
  //      inputKey = keyEventData.inputKey,
  //      isUp = keyEventData.keyEventType.IsUp,
  //      config = _session.config,
  //    }.ProcessKey();

  //  }


  //}

  //CS
  class App
  {
    private Session _session;
    private KeyboardHook keyboardHook;
    private bool isSending;
    private Action<State> actionRefreshIcon;
    private Action actionExit;

    public App(Action<State> actionRefreshIcon, Action actionExit)
    {
      isSending = false;
      _session = DomainUtils.CreateSession(() => File.ReadAllText(DomainUtils.filePathNew));
      SetupKeyboardHooks();
      this.actionRefreshIcon = actionRefreshIcon;
      this.actionExit = actionExit;
    }

    private void SetupKeyboardHooks()
    {
      keyboardHook = new KeyboardHook();
      keyboardHook.KeyboardPressed += OnKeyPressed;
    }

    private KeyEventData CreateKetEventData(KeyboardHookEvent e)
    {
      KeyEventType keyEventType = KeyEventType.Down;
      if (e.KeyboardState == KeyboardState.KeyUp) keyEventType = KeyEventType.Up;
      if (e.KeyboardState == KeyboardState.KeyUp) keyEventType = KeyEventType.Up;
      var key = (Keys)e.KeyboardData.VirtualCode;
      var inputKey = DomainUtils.CreateInputKey(key.ToString());
      return new KeyEventData { inputKey = inputKey, keyEventType = keyEventType };
    }

    private void OnKeyPressed(object sender, KeyboardHookEvent e)
    {
      OnKeyPressed(e);
    }

    private void OnKeyPressed(KeyboardHookEvent e)
    {
      if (isSending) return;

      Keys key = (Keys)e.KeyboardData.VirtualCode;
      if (Control.IsKeyLocked(Keys.CapsLock) && key == Keys.Capital) return;

      KeyEventData keyEventData = CreateKetEventData(e);

      var output = Work(keyEventData);
      if (output == null) return;

      e.Handled = output.preventKeyProcess;

      if (!string.IsNullOrEmpty(output.send))
      {
        isSending = true;
        SendKeys.Send(output.send);
        isSending = false;
      }

      actionRefreshIcon(_session.appState.state);

      if (TryOpenSettingsFile(key, output.appState)) { e.Handled = true; return; }
      if (TryExitApp(key, output.appState)) { e.Handled = true; return; }

    }

    private KeysEngineResult Work(KeyEventData keyEventData)
    {

      KeysEngineResult output = ProcessKey(keyEventData);

      if (output == null) return null;

      _session = new Session { config = _session.config, appState = output.appState };

      return output;

    }

    private KeysEngineResult ProcessKey(KeyEventData keyEventData)
    {
      bool isUp = keyEventData.keyEventType == KeyEventType.Up;

      var processor = new DoKey.CoreCS.KeysProcessor.KeysProcessor(_session.appState,
        keyEventData.inputKey,
        isUp,
        _session.config);

      return processor.ProcessKey();

    }

    private bool TryExitApp(Keys key, AppState appState)
    {
      if (!appState.modificators.caps) return false;
      if (key != Keys.Back) return false;
      actionExit();
      return true;
    }

    private bool TryOpenSettingsFile(Keys key, AppState appState)
    {
      if (!appState.modificators.caps) return false;
      if (key != Keys.Oem2) return false;
      OpenSettings();
      return true;

    }

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

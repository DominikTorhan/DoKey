using System;

namespace DoKey.CoreCS
{

  public class AppProcessor
  {
    private readonly Session _session;
    private readonly Action<State> _actionRefreshIcon;
    private readonly Action _actionExit;
    private readonly Action<string> _actionLog;
    private readonly Action<string> _actionSend;
    private readonly Action _actionShowConfigFile;
    private readonly KeyEvent _keyEvent;

    public AppProcessor(AppConfig appConfig, Session session, KeyEvent keyEvent)
    {
      _session = session;
      _keyEvent = keyEvent;
      _actionRefreshIcon = appConfig.actionRefreshIcon;
      _actionExit = appConfig.actionExit;
      _actionSend = appConfig.actionSend;
      _actionShowConfigFile = appConfig.actionShowConfigFile;
      _actionLog = appConfig.actionLog;
    }

    public KeysProcessorResult PerformKeyPress()
    {
      var output = ProcessKey();
      if (output == null) return null;
      TryRefreshStateIcon(output.appState.state);
      if (TryRunCommandKey(output.send)) return output;
      if (output.send == "") return output;
      _actionLog($"{_keyEvent} send: {output.send}");
      _actionSend(output.send);
      return output;
    }

    private void TryRefreshStateIcon(State state)
    {
      if (_session.appState.state == state) return;
      _actionRefreshIcon?.Invoke(state);
    }

    private bool TryRunCommandKey(string send)
    {
      if (send == "#exit") { _actionExit(); return true; }
      if (send == "#config") { _actionShowConfigFile(); return true; }
      return false;
    }

    private KeysProcessorResult ProcessKey()
    {
      var inputKey = DomainUtils.CreateInputKey(_keyEvent.key);
      var processor = new KeysProcessor.KeysProcessor(_session.appState,
        inputKey,
        _keyEvent.isUp,
        _session.config);
      return processor.ProcessKey();
    }

  }

}

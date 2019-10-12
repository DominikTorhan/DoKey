using System;

namespace DoKey.CoreCS
{

  public class AppProcessor
  {
    private readonly Session session;
    private readonly Action<State> actionRefreshIcon;
    private readonly Action actionExit;
    private readonly Action<string> actionLog;
    private readonly Action<string> actionSend;
    private readonly Action actionShowConfigFile;
    private readonly KeyEvent keyEvent;

    public AppProcessor(AppConfig appConfig, Session session, KeyEvent keyEvent)
    {
      this.session = session;
      this.keyEvent = keyEvent;
      actionRefreshIcon = appConfig.actionRefreshIcon;
      actionExit = appConfig.actionExit;
      actionSend = appConfig.actionSend;
      actionShowConfigFile = appConfig.actionShowConfigFile;
      actionLog = appConfig.actionLog;
    }

    public KeysEngineResult PerformKeyPress()
    {
      var output = ProcessKey();
      if (output == null) return null;
      TryRefreshStateIcon(output.appState.state);
      if (TryRunCommandKey(output.send)) return output;
      if (output.send == "") return output;
      actionLog($"{keyEvent} send: {output.send}");
      actionSend(output.send);
      return output;
    }

    private void TryRefreshStateIcon(State state)
    {
      if (session.appState.state == state) return;
      actionRefreshIcon?.Invoke(state);
    }

    private bool TryRunCommandKey(string send)
    {
      if (send == "#exit") { actionExit(); return true; }
      if (send == "#config") { actionShowConfigFile(); return true; }
      return false;
    }

    private KeysEngineResult ProcessKey()
    {
      var inputKey = DomainUtils.CreateInputKey(keyEvent.key);
      var processor = new KeysProcessor.KeysProcessor(session.appState,
        inputKey,
        keyEvent.isUp,
        session.config);
      return processor.ProcessKey();
    }

  }

}

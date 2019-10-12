using System;

namespace DoKey.CoreCS
{
  public class AppConfig
  {
    public Action<State> actionRefreshIcon;
    public Action actionExit;
    public Action actionShowConfigFile;
    public Action<string> actionLog;
    public Func<string> funcGetConfigText;
    public Action<string> actionSend;
  }

}

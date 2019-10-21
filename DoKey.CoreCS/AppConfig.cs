using System;

namespace DoKey.CoreCS
{
  public class AppConfig
  {
    public Action<State> actionRefreshIcon { get; set; }
    public Action actionExit { get; set; }
    public Action actionShowConfigFile { get; set; }
    public Action<string> actionLog { get; set; }
    public Func<string> funcGetConfigText { get; set; }
    public Action<string> actionSend { get; set; }
  }

}

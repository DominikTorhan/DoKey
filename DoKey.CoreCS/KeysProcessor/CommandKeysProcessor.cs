using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoKey.CoreCS.KeysProcessor
{
  public class CommandKeysProcessor
  {
    private readonly InputKey inputKey;
    private readonly AppState appState;
    private readonly IEnumerable<CommandKey> commandKeys;

    public CommandKeysProcessor(InputKey inputKey, AppState appState, IEnumerable<CommandKey> commandKeys)
    {
      this.inputKey = inputKey;
      this.appState = appState;
      this.commandKeys = commandKeys;
    }
    public KeysEngineResult Process()
    {
      return ProcessNormalAndInsertWithCapital();

    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {
      if (!appState.modificators.caps) return null;
      if (appState.state == State.Off) return null;

      var commandKey = GetCommandKeyWithCapital();
      if (commandKey == null) return null;
      if (commandKey.run == "") return null;
      var nextAppState = new AppState
      {
        state = appState.state,
        modificators = appState.modificators,
        firstStep = "",
        preventEscOnCapsUp = true
      };
      return new KeysEngineResult
      {
        appState = nextAppState,
        send = commandKey.run,
        preventKeyProcess = true
      };
    }


    private CommandKey GetCommandKeyWithCapital()
    {
      return commandKeys.FirstOrDefault(key => key.trigger == inputKey.key);
    }

  }
}

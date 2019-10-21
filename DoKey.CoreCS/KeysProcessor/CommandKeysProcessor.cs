using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class CommandKeysProcessor
  {
    private readonly InputKey _inputKey;
    private readonly AppState _appState;
    private readonly IEnumerable<CommandKey> _commandKeys;

    public CommandKeysProcessor(InputKey inputKey, AppState appState, IEnumerable<CommandKey> commandKeys)
    {
      _inputKey = inputKey;
      _appState = appState;
      _commandKeys = commandKeys;
    }

    public KeysProcessorResult Process()
    {
      return ProcessNormalAndInsertWithCapital();
    }

    private KeysProcessorResult ProcessNormalAndInsertWithCapital()
    {
      if (!_appState.modificators.caps) return null;
      if (_appState.state == State.Off) return null;
      var commandKey = GetCommandKeyWithCapital();
      if (commandKey == null) return null;
      if (commandKey.run == "") return null;
      var nextAppState = new AppState(_appState.state, _appState.modificators, "", true);
      return new KeysProcessorResult(nextAppState, commandKey.run, true);
    }

    private CommandKey GetCommandKeyWithCapital()
    {
      return _commandKeys.FirstOrDefault(key => key.trigger == _inputKey.key);
    }

  }
}

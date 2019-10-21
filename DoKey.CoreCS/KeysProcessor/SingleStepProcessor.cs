using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class SingleStepProcessor
  {
    private readonly InputKey _inputKey;
    private readonly AppState _appState;
    private readonly IEnumerable<MappedKey> _mappedKeys;

    public SingleStepProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      _inputKey = inputKey;
      _appState = appState;
      _mappedKeys = mappedKeys;
    }

    public KeysProcessorResult TryGetSingleStep()
    {
      var mappedKey = TryGetMappedKey();
      if (mappedKey == null) return null;
      return new KeysProcessorResult(_appState, mappedKey.send, true);
    }

    private MappedKey TryGetMappedKey()
    {
      if (_appState.modificators.caps) return null;
      if (_appState.state != State.Normal) return null;
      if (_appState.modificators.win) return null;
      if (_appState.firstStep != "") return null;
      if (_inputKey.isFirstStep) return null;
      var mappedKey = _mappedKeys.FirstOrDefault(key => !key.isCaps && key.trigger == _inputKey.key);
      return mappedKey;
    }

  }
}

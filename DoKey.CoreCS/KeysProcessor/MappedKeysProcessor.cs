using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class MappedKeysProcessor
  {
    private readonly InputKey _inputKey;
    private readonly AppState _appState;
    private readonly IEnumerable<MappedKey> _mappedKeys;

    public MappedKeysProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      _inputKey = inputKey;
      _appState = appState;
      _mappedKeys = mappedKeys;
    }

    public KeysProcessorResult Process()
    {
      return ProcessNormalAndInsertWithCapital();

    }

    private KeysProcessorResult ProcessNormalAndInsertWithCapital()
    {
      if (!_appState.modificators.caps) return null;
      if (_appState.state == State.Off) return null;

      var mappedKey = GetMappedKeyNormalAndInsertWithCapital();
      if (mappedKey == null) return null;
      if (mappedKey.send == "") return null;
      var nextAppState = new AppState(_appState.state, _appState.modificators, "", true);
      return new KeysProcessorResult(nextAppState, mappedKey.send, true);
    }

    private MappedKey GetMappedKeyNormalAndInsertWithCapital()
    {
      return _mappedKeys.FirstOrDefault(key => key.isCaps && key.trigger == _inputKey.key);
    }

  }
}

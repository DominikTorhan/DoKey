using System.Collections.Generic;
using System.Linq;

namespace DoKey.CoreCS.KeysProcessor
{
  public class DoubleStepProcessor
  {
    private readonly InputKey _inputKey;
    private readonly AppState _appState;
    private readonly IEnumerable<MappedKey> _mappedKeys;

    public DoubleStepProcessor(InputKey inputKey, AppState appState, IEnumerable<MappedKey> mappedKeys)
    {
      _inputKey = inputKey;
      _appState = appState;
      _mappedKeys = mappedKeys;
    }

    public KeysProcessorResult TryGetSingleStep()
    {
      if (_appState.state != State.Normal) return null;
      if (_appState.modificators.caps) return null;
      if (_appState.modificators.win) return null;
      var result = TryProcessFirstStep();
      if (result != null) return result;
      if (_appState.firstStep == "") return null;
      var send = TryGetMappedKey();
      return new KeysProcessorResult(CreateAppStateWithFirstStep(""), send, true);
    }

    private AppState CreateAppStateWithFirstStep(string firstStep)
    {
      return new AppState(_appState.state, _appState.modificators, firstStep, _appState.preventEscOnCapsUp);
    }

    private KeysProcessorResult TryProcessFirstStep()
    {
      if (_appState.firstStep != "") return null;
      if (!_inputKey.isFirstStep) return null;
      var nextAppState = CreateAppStateWithFirstStep(_inputKey.key);
      return new KeysProcessorResult(nextAppState, "", _inputKey.isLetterOrDigit);
    }

    private string TryGetMappedKey()
    {
      var trigger = _appState.firstStep + _inputKey.key;
      var mappedKey = _mappedKeys.FirstOrDefault(key => !key.isCaps && key.trigger == trigger);
      return mappedKey?.send ?? "";
    }

  }
}

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
    public KeysEngineResult Process()
    {
      if (CanProcessNormalMode()) return ProcessNormalMode();
      return ProcessNormalAndInsertWithCapital();
    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {
      var mappedKey = GetMappedKeyNormalAndInsertWithCapital();
      if (mappedKey == null) return null;
      if (mappedKey.send == "") return null;
        var nextAppState = new AppState
      {
        state = _appState.state,
        modificators = _appState.modificators,
        firstStep = "",
        preventEscOnCapsUp = true
      };
      return new KeysEngineResult
      {
        appState = nextAppState,
        send = mappedKey.send,
        preventKeyProcess = true
      };
    }


    private MappedKey GetMappedKeyNormalAndInsertWithCapital()
    {
      if (!_appState.modificators.caps) return CreateEmptyMappedKey();
      return GetMappedKey();
    }

    private string GetTrigger() => _appState.firstStep + _inputKey.key;

    private MappedKey GetMappedKey()
    {
      if (_appState.state == State.Off) return null;
      var trigger = GetTrigger();
      return _mappedKeys.FirstOrDefault(key => key.isCaps == _appState.modificators.caps && key.trigger == trigger);
    }

    private bool CanProcessNormalMode()
    {
      if (_appState.state != State.Normal) return false;
      if (_appState.modificators.win) return false;
      return true;
    }

    private MappedKey GetMappedKeyNormal()
    {
      if (IsDownFirstStep()) return null;
      return GetMappedKey();
    }

    private MappedKey CreateEmptyMappedKey()
    {
      return new MappedKey { isCaps = false, send = "", trigger = "" };
    }

    private bool IsDownFirstStep() => _appState.firstStep == "" && DomainUtils.IsTwoStep(_inputKey.key);

    private KeysEngineResult ProcessNormalMode()
    {
      var isDownFirstStep = _appState.firstStep == "" && DomainUtils.IsTwoStep(_inputKey.key);
      var nextFirstStep = isDownFirstStep ? _inputKey.key : "";
      var mappedKey = GetMappedKeyNormal();
      if (mappedKey == null) mappedKey = CreateEmptyMappedKey();
      var preventKeyProcess = _inputKey.isLetterOrDigit || mappedKey.send != "";
      var nextAppState = new AppState
      {
        state = _appState.state,
        modificators = _appState.modificators,
        firstStep = nextFirstStep,
        preventEscOnCapsUp = _appState.preventEscOnCapsUp
      };
      return new KeysEngineResult
      {
        appState = nextAppState,
        send = mappedKey.send,
        preventKeyProcess = preventKeyProcess
      };
    }

  }
}

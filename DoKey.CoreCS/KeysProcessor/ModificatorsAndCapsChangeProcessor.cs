using System;
using System.Collections.Generic;
using System.Text;

namespace DoKey.CoreCS.KeysProcessor
{
public  class ModificatorsAndCapsChangeProcessor
  {
    public readonly AppState _appState;
    public readonly InputKey _inputKey;
    public readonly bool _isUp;

    public ModificatorsAndCapsChangeProcessor(AppState appState, InputKey inputKey, bool isUp)
    {
      _appState = appState;
      _inputKey = inputKey;
      _isUp = isUp;
    }

    public KeysEngineResult ProcessKey()
    {

      if (_inputKey.isCaps) return ProcessCapital();
      if (_inputKey.isModif) return ProcessModificators();

      if (_isUp) return CreateEmptyKeysEngineResult();

      return null;

    }

    private KeysEngineResult CreateEmptyKeysEngineResult()
    {
      return new KeysEngineResult
      {
        appState = _appState,
        send = "",
        preventKeyProcess = false
      };
    }

    private Modificators GetNextModificators()
    {
      return new ModificatorsManager(_appState.modificators, _inputKey, _isUp).GetNextModificators();
    }

    private KeysEngineResult ProcessModificators()
    {
      var modificators = GetNextModificators();
      var appState = new AppState
      {
        state = _appState.state,
        modificators = modificators,
        firstStep = _appState.firstStep,
        preventEscOnCapsUp = _appState.preventEscOnCapsUp
      };
      return new KeysEngineResult
      {
        appState = appState,
        send = "",
        preventKeyProcess = false
      };
    }

    private KeysEngineResult ProcessCapital()
    {
      var sendKeys = "";
      var preventEscOnNextCapitalUp = _appState.preventEscOnCapsUp;
      if (_isUp)
      {
        if (_appState.preventEscOnCapsUp)
        {
          preventEscOnNextCapitalUp = false;
        }
        else
        {
          sendKeys = "{ESC}";
        }
      }
      var modificators = GetNextModificators();
      var appState = new AppState
      {
        state = _appState.state,
        modificators = modificators,
        firstStep = "",
        preventEscOnCapsUp = preventEscOnNextCapitalUp
      };
      return new KeysEngineResult
      {
        appState = appState,
        send = sendKeys,
        preventKeyProcess = true
      };
    }

  }
}

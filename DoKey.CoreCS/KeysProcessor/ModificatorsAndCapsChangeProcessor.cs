using System;
using System.Collections.Generic;
using System.Text;

namespace DoKey.CoreCS.KeysProcessor
{
public  class ModificatorsAndCapsChangeProcessor
  {
    public readonly AppState appState;
    public readonly InputKey inputKey;
    public readonly bool isUp;

    public ModificatorsAndCapsChangeProcessor(AppState appState, InputKey inputKey, bool isUp)
    {
      this.appState = appState;
      this.inputKey = inputKey;
      this.isUp = isUp;
    }

    public KeysEngineResult ProcessKey()
    {
      if (inputKey.isCaps) return ProcessCapital();
      if (inputKey.isModif) return ProcessModificators();
      return null;
    }

    private Modificators GetNextModificators()
    {
      return new ModificatorsManager(appState.modificators, inputKey, isUp).GetNextModificators();
    }

    private KeysEngineResult ProcessModificators()
    {
      var modificators = GetNextModificators();
      var appState = new AppState
      {
        state = this.appState.state,
        modificators = modificators,
        firstStep = this.appState.firstStep,
        preventEscOnCapsUp = this.appState.preventEscOnCapsUp
      };
      return new KeysEngineResult
      {
        appState = appState,
        send = "",
        preventKeyProcess = false
      };
    }

    private string GetSendESC()
    {
      if (!isUp) return "";
      if (appState.preventEscOnCapsUp) return "";
      return "{ESC}";
    }

    private bool GetPreventEscOnCapsUp()
    {
      if (isUp && appState.preventEscOnCapsUp) return false;
      return appState.preventEscOnCapsUp;
    }

    private KeysEngineResult ProcessCapital()
    {
      var sendKeys = GetSendESC();
      var preventEscOnNextCapitalUp = GetPreventEscOnCapsUp();
      var modificators = GetNextModificators();
      var appState = new AppState
      {
        state = this.appState.state,
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoKey.Core;
using static DoKey.Core.Domain;

namespace DoKey
{

  public class KeysEngine
  {

    public Config config { get; set; }
    public AppState AppState { get; set; }
    public InputKey inputKey { get; set; }
    public bool isUp { get; set; }

    public KeysEngineResult ProcessKey()
    {

      if (inputKey.isCaps) return ProcessCapital();
      if (inputKey.isModif) return ProcessModificators();

      var output = ProcessSetModeOff();
      if (output != null) return output;

      output = ProcessModeChange();
      if (output != null) return output;

      output = ProcessEsc();
      if (output != null) return output;

      if (AppState.state == State.Off) return new KeysEngineResult(AppState, "", false);

      output = ProcessNormalAndInsertWithCapital();
      if (output != null) return output;

      output = ProcessNormalMode();
      if (output != null) return output;

      return new KeysEngineResult(AppState, "", false);
    }

    private KeysEngineResult ProcessModificators()
    {

      //var modificators = this.AppState.Modificators.GetNextModificators(inputKey, isUp);
      var modificators = ModificatorsOperations.GetNextModificators(this.AppState.modificators, inputKey, isUp);

      return new KeysEngineResult(new AppState(this.AppState.state, modificators, this.AppState.firstStep, this.AppState.preventEscOnCapsUp), "", false);

    }

    private KeysEngineResult ProcessCapital()
    {

      var sendKeys = "";
      var preventEscOnNextCapitalUp = AppState.preventEscOnCapsUp;

      if (isUp)
      {
        if (AppState.preventEscOnCapsUp)
        {
          preventEscOnNextCapitalUp = false;
        }
        else
        {
          sendKeys = "{ESC}";
        }
      }

      var modif = ModificatorsOperations.GetNextModificators(this.AppState.modificators, inputKey, isUp);

      return new KeysEngineResult(new AppState(this.AppState.state, modif, "", preventEscOnNextCapitalUp), sendKeys, true);

    }
    
    private KeysEngineResult ProcessModeChange()
    {

      if (inputKey.key != DoKey.Core.Domain.modeChangeKey) return null;
      if (!AppState.modificators.caps) return null;
      if (isUp) return null;

      return new KeysEngineResult(new AppState(GetNextState(AppState.state), this.AppState.modificators, "", true), "", true);

    }

    private KeysEngineResult ProcessEsc()
    {

      if (!inputKey.isEsc) return null;
      if (AppState.state != State.Insert) return null;
      if (isUp) return null;
      if (AppState.modificators.caps) return null;

      return new KeysEngineResult(new AppState(GetPrevState(AppState.state), this.AppState.modificators, "", this.AppState.preventEscOnCapsUp), 
        "", true);

    }

    private KeysEngineResult ProcessNormalMode()
    {

      if (AppState.state != State.Normal) return null;
      if (isUp) return null;
      if (AppState.modificators.win) return null;

      var isDownFirstStep = AppState.firstStep == "" && DomainOperations.IsTwoStep(inputKey.key);

      var firstStepNext = isDownFirstStep ? inputKey.key : "";

      var sendDoKey = DoKey.Core.KeysEngine.GetMappedKeyNormal(AppState.firstStep, inputKey.key.ToString(), config.mappedKeys);

      var preventKeyProcess = inputKey.isLetterOrDigit || sendDoKey.send != "";

      return new KeysEngineResult(AppState = new AppState(this.AppState.state, this.AppState.modificators, firstStepNext, this.AppState.preventEscOnCapsUp),
        sendDoKey.send, preventKeyProcess);

    }

    private KeysEngineResult ProcessSetModeOff()
    {

      if (!AppState.modificators.caps) return null;
      if (isUp) return null;
      if (inputKey.key != DoKey.Core.Domain.modeOffKey) return null;

      return new KeysEngineResult(AppState = new AppState(State.Off, this.AppState.modificators, "", true), "", true);

    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {

      var sendKeys = DoKey.Core.KeysEngine.GetMappedKeyNormalAndInsertWithCapital(AppState, inputKey.key.ToString(), config.mappedKeys, isUp);

      if (sendKeys.send == "") return null;

      return new KeysEngineResult(new AppState(this.AppState.state, this.AppState.modificators, "", true), sendKeys.send, true);

    }

    public static State GetNextState(State state)
    {

      if (state == State.Insert) return State.Insert;
      if (state == State.Normal) return State.Insert;

      return State.Normal;

    }

    public static State GetPrevState(State state)
    {

      if (state == State.Insert) return State.Normal;
      if (state == State.Normal) return State.Normal;

      return State.Off;

    }

  }
}


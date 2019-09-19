using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoKey.FS;
using static DoKey.FS.Domain;

namespace DoKey
{

  public class KeysEngine
  {

    public Configuration Configuration { get; set; }
    public KeyEventData KeyEventData { get; set; }
    public AppState AppState { get; set; }

    private InputKey inputKey => KeyEventData.InputKey;
    private string keys => inputKey.Key;
    private bool isUp => KeyEventData.KeyEventType.IsUp;
    private bool isCapital => AppState.modificators.caps;

    public KeysEngineResult ProcessKey()
    {

      if (inputKey.IsCapital) return ProcessCapital();
      if (inputKey.IsModifier) return ProcessModificators();

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

      if (keys != DoKey.FS.Domain.modeChangeKey) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      return new KeysEngineResult(new AppState(Utils.GetNextState(AppState.state), this.AppState.modificators, "", true), "", true);

    }

    private KeysEngineResult ProcessEsc()
    {

      if (!inputKey.IsEsc) return null;
      if (AppState.state != State.Insert) return null;
      if (isUp) return null;
      if (isCapital) return null;

      return new KeysEngineResult(new AppState(Utils.GetPrevState(AppState.state), this.AppState.modificators, "", this.AppState.preventEscOnCapsUp), 
        "", true);

    }

    private KeysEngineResult ProcessNormalMode()
    {

      if (AppState.state != State.Normal) return null;
      if (isUp) return null;
      if (AppState.modificators.win) return null;

      var isDownFirstStep = AppState.firstStep == "" && Configuration.IsTwoStep(keys);

      var firstStepNext = isDownFirstStep ? keys : "";

      var sendDoKey = GetSendDoKey(isDownFirstStep);

      var preventKeyProcess = inputKey.IsLetterOrDigit || sendDoKey.send != "";

      return new KeysEngineResult(AppState = new AppState(this.AppState.state, this.AppState.modificators, firstStepNext, this.AppState.preventEscOnCapsUp),
        sendDoKey.send, preventKeyProcess);

    }

    private MappedKey GetSendDoKey(bool isDownFirstStep)
    {

      if (isDownFirstStep) return new MappedKey("", "", false);

      var trigger = AppState.firstStep + keys.ToString();

      var doKey = Configuration.GetSendKeyNormal(trigger);

      return doKey;

    }

    private KeysEngineResult ProcessSetModeOff()
    {

      if (!isCapital) return null;
      if (isUp) return null;
      if (keys != DoKey.FS.Domain.modeOffKey) return null;

      return new KeysEngineResult(AppState = new AppState(State.Off, this.AppState.modificators, "", true), "", true);

    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {

      if (AppState.state == State.Off) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = Configuration.GetSendKeyCaps(keys.ToString());

      if (sendKeys.send == "") return null;

      return new KeysEngineResult(new AppState(this.AppState.state, this.AppState.modificators, "", true), sendKeys.send, true);

    }

  }
}


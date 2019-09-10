using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoKey.FS;
using static DoKey.FS.Domain;

namespace TrayApp2
{

  public class cKeysEngine
  {

    public Configuration Configuration { get; set; }
    public KeyEventData KeyEventData { get; set; }
    public AppState AppState { get; set; }

    private InputKey inputKey => KeyEventData.InputKey;
    private string keys => inputKey.Key;
    private bool isUp => KeyEventData.KeyEventType.IsUp;
    private bool isCapital => AppState.Modificators.Caps;

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

      if (AppState.State == State.Off) return new KeysEngineResult(AppState);

      output = ProcessNormalAndInsertWithCapital();
      if (output != null) return output;

      output = ProcessNormalMode();
      if (output != null) return output;

      return new KeysEngineResult(AppState);
    }

    private KeysEngineResult ProcessModificators()
    {

      var modificators = this.AppState.Modificators.GetNextModificators(inputKey, isUp);

      return new KeysEngineResult(new AppState(this.AppState.State, modificators, this.AppState.FirstStep, this.AppState.PreventEscOnCapsUp));

    }

    private KeysEngineResult ProcessCapital()
    {

      var sendKeys = "";
      var preventEscOnNextCapitalUp = AppState.PreventEscOnCapsUp;

      if (isUp)
      {
        if (AppState.PreventEscOnCapsUp)
        {
          preventEscOnNextCapitalUp = false;
        }
        else
        {
          sendKeys = "{ESC}";
        }
      }

      var modif = this.AppState.Modificators.GetNextModificators(inputKey, isUp);

      return new KeysEngineResult(new AppState(this.AppState.State, modif, "", preventEscOnNextCapitalUp), new SendKey("", sendKeys, false), true);

    }
    
    private KeysEngineResult ProcessModeChange()
    {

      if (keys != Configuration.ModeChangeKey) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      return new KeysEngineResult(new AppState(cUtils.GetNextState(AppState.State), this.AppState.Modificators, "", true), new SendKey("", "", false), true);

    }

    private KeysEngineResult ProcessEsc()
    {

      if (!inputKey.IsEsc) return null;
      if (AppState.State != State.Insert) return null;
      if (isUp) return null;
      if (isCapital) return null;

      return new KeysEngineResult(new AppState(cUtils.GetPrevState(AppState.State), this.AppState.Modificators, "", this.AppState.PreventEscOnCapsUp), 
        new SendKey("", "", false), true);

    }

    private KeysEngineResult ProcessNormalMode()
    {

      if (AppState.State != State.Normal) return null;
      if (isUp) return null;
      if (AppState.Modificators.Win) return null;

      var isDownFirstStep = AppState.FirstStep == "" && Configuration.IsTwoStep(keys);

      var firstStepNext = isDownFirstStep ? keys : "";

      var sendDoKey = GetSendDoKey(isDownFirstStep);

      var preventKeyProcess = inputKey.IsLetterOrDigit || sendDoKey.send != "";

      return new KeysEngineResult(AppState = new AppState(this.AppState.State, this.AppState.Modificators, firstStepNext, this.AppState.PreventEscOnCapsUp),
        sendDoKey, preventKeyProcess);

    }

    private SendKey GetSendDoKey(bool isDownFirstStep)
    {

      if (isDownFirstStep) return new SendKey("", "", false);

      var trigger = AppState.FirstStep + keys.ToString();

      var doKey = Configuration.GetSendKeyNormal(trigger);

      return doKey;

    }

    private KeysEngineResult ProcessSetModeOff()
    {

      if (!isCapital) return null;
      if (isUp) return null;
      if (keys != Configuration.ModeOffKey) return null;

      return new KeysEngineResult(AppState = new AppState(State.Off, this.AppState.Modificators, "", true), new SendKey("", "", false), true);

    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {

      if (AppState.State == State.Off) return null;
      if (!isCapital) return null;
      if (isUp) return null;

      var sendKeys = Configuration.GetSendKeyCaps(keys.ToString());

      if (sendKeys.send == "") return null;

      return new KeysEngineResult(new AppState(this.AppState.State, this.AppState.Modificators, "", true), sendKeys, true);

    }

  }
}


using DoKey.Core;
using static DoKey.Core.Domain;

namespace DoKey
{

  public class KeysEngine
  {

    public Config config { get; set; }
    public AppState AppStateX { get; set; }
    public InputKey inputKey { get; set; }
    public bool isUp { get; set; }

    public KeysEngineResult ProcessKey()
    {

      if (inputKey.isCaps) return ProcessCapital();
      if (inputKey.isModif) return ProcessModificators();

      if (isUp) return new KeysEngineResult(AppStateX, "", false);

      //------------------
      var appState = ProcessStateChange();
      if (appState != null) return DomainOperations.CreateEngineResultChangeAppState(appState);
      //------------------------

      if (AppStateX.state == State.Off) return new KeysEngineResult(AppStateX, "", false);

      var output = ProcessNormalAndInsertWithCapital();
      if (output != null) return output;

      return Core.KeysEngine.ProcessKey(AppStateX, inputKey, config.mappedKeys);

    }

    private AppState ProcessStateChange()
    {
      if (AppStateX.modificators.caps)
      {
        var x = Core.KeysEngine.GetNextAppStateByStateChange(inputKey.key, AppStateX);
        try        {          return x.Value;        }
        catch        {          return null;        }
      }
      else
      {
        var x = Core.KeysEngine.GetNextAppStateByESC(inputKey, AppStateX);
        try { return x.Value; }
        catch { return null; }
      }
    }

    private KeysEngineResult ProcessModificators()
    {

      var modificators = ModificatorsOperations.GetNextModificators(AppStateX.modificators, inputKey, isUp);

      return new KeysEngineResult(new AppState(AppStateX.state, modificators, AppStateX.firstStep, AppStateX.preventEscOnCapsUp), "", false);

    }

    private KeysEngineResult ProcessCapital()
    {

      var sendKeys = "";
      var preventEscOnNextCapitalUp = AppStateX.preventEscOnCapsUp;

      if (isUp)
      {
        if (AppStateX.preventEscOnCapsUp)
        {
          preventEscOnNextCapitalUp = false;
        }
        else
        {
          sendKeys = "{ESC}";
        }
      }

      var modif = ModificatorsOperations.GetNextModificators(AppStateX.modificators, inputKey, isUp);

      return new KeysEngineResult(new AppState(AppStateX.state, modif, "", preventEscOnNextCapitalUp), sendKeys, true);

    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {

      var sendKeys = DoKey.Core.KeysEngine.GetMappedKeyNormalAndInsertWithCapital(AppStateX, inputKey.key.ToString(), config.mappedKeys);

      if (sendKeys.send == "") return null;

      return new KeysEngineResult(new AppState(AppStateX.state, AppStateX.modificators, "", true), sendKeys.send, true);

    }

  }
}


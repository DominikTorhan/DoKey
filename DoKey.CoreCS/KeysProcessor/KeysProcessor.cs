namespace DoKey.CoreCS.KeysProcessor
{
  public class KeysProcessor
  {
    public readonly AppState _appState;
    public readonly InputKey _inputKey;
    public readonly bool _isUp;
    public readonly Config _config;

    public KeysProcessor(AppState appState, InputKey inputKey, bool isUp, Config config)
    {
      _appState = appState;
      _inputKey = inputKey;
      _isUp = isUp;
      _config = config;
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

    public KeysEngineResult CreateEngineResultChangeAppState(AppState appState)
    {
      return new KeysEngineResult
      {
        appState = appState,
        send = "",
        preventKeyProcess = true
      };
    }


    public KeysEngineResult ProcessKey()
    {
      var result = ProcessModifChange();
      if (result == null) return result;
      //------------------
      result = ProcessStateChange();
      if (result != null) return result;
      //------------------------

      if (_appState.state == State.Off) return CreateEmptyKeysEngineResult();

      result = ProcessNormalAndInsertWithCapital();
      if (result != null) return result;

      return null;

    }

    private KeysEngineResult ProcessModifChange()
    {
      return new ModificatorsAndCapsChangeProcessor(_appState, _inputKey, _isUp).ProcessKey();
    }

    private KeysEngineResult ProcessStateChange()
    {
      var appState = new StateChangeProcessor(_appState, _inputKey).ProcessStateChange();
      if (appState == null) return null;
      return CreateEngineResultChangeAppState(appState);
    }

    private KeysEngineResult ProcessNormalAndInsertWithCapital()
    {
      return null;
      //var sendKeys = DoKey..GetMappedKeyNormalAndInsertWithCapital(_appState, _inputKey.key.ToString(), config.mappedKeys);
      //if (sendKeys.send == "") return null;
      //return new KeysEngineResult(new AppState(_appState.state, _appState.modificators, "", true), sendKeys.send, true);
    }

  }
}

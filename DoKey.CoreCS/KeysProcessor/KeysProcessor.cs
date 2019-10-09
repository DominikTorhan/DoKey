namespace DoKey.CoreCS.KeysProcessor
{
  public class KeysProcessor
  {
    public readonly AppState appState;
    public readonly InputKey inputKey;
    public readonly bool isUp;
    public readonly Config config;

    public KeysProcessor(AppState appState, InputKey inputKey, bool isUp, Config config)
    {
      this.appState = appState;
      this.inputKey = inputKey;
      this.isUp = isUp;
      this.config = config;
    }

    public KeysEngineResult ProcessKey()
    {
      //modificatorsChange
      var result = ProcessModifChange();
      if (result != null) return result;
      if (isUp) return CreateEmptyKeysEngineResult();
      //stateChange
      result = ProcessStateChange();
      if (result != null) return result;
      if (appState.state == State.Off) return CreateEmptyKeysEngineResult();
      //sigleStepKey
      result = new SingleStepProcessor(inputKey, appState, config.mappedKeys).TryGetSingleStep();
      if (result != null) return result;
      //doubleStepKey
      result = new DoubleStepProcessor(inputKey, appState, config.mappedKeys).TryGetSingleStep();
      if (result != null) return result;
      //capitalKey
      return new MappedKeysProcessor(inputKey, appState, config.mappedKeys).Process();
    }

    private KeysEngineResult ProcessModifChange()
    {
      var processor = new ModificatorsAndCapsChangeProcessor(appState, inputKey, isUp);
      return processor.ProcessKey();
    }

    private KeysEngineResult ProcessStateChange()
    {
      var appState = new StateChangeProcessor(this.appState, inputKey).ProcessStateChange();
      if (appState == null) return null;
      return CreateEngineResultChangeAppState(appState);
    }

    private KeysEngineResult CreateEmptyKeysEngineResult()
    {
      return new KeysEngineResult
      {
        appState = appState,
        send = "",
        preventKeyProcess = false
      };
    }

    private KeysEngineResult CreateEngineResultChangeAppState(AppState appState)
    {
      return new KeysEngineResult
      {
        appState = appState,
        send = "",
        preventKeyProcess = true
      };
    }

  }
}

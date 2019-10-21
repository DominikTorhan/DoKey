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

    public KeysProcessorResult ProcessKey()
    {
      //modificatorsChange
      var result = ProcessModifChange();
      if (result != null) return result;
      if (_isUp) return CreateEmptyKeysEngineResult();
      //stateChange
      result = ProcessStateChange();
      if (result != null) return result;
      if (_appState.state == State.Off) return CreateEmptyKeysEngineResult();
      //sigleStepKey
      result = new SingleStepProcessor(_inputKey, _appState, _config.mappedKeys).TryGetSingleStep();
      if (result != null) return result;
      //doubleStepKey
      result = new DoubleStepProcessor(_inputKey, _appState, _config.mappedKeys).TryGetSingleStep();
      if (result != null) return result;
      //commandKey
      result = new CommandKeysProcessor(_inputKey, _appState, _config.commandKeys).Process();
      if (result != null) return result;
      //capitalKey
      return new MappedKeysProcessor(_inputKey, _appState, _config.mappedKeys).Process();
    }

    private KeysProcessorResult ProcessModifChange()
    {
      var processor = new ModificatorsAndCapsChangeProcessor(_appState, _inputKey, _isUp);
      return processor.ProcessKey();
    }

    private KeysProcessorResult ProcessStateChange()
    {
      var appState = new StateChangeProcessor(_appState, _inputKey).ProcessStateChange();
      if (appState == null) return null;
      return new KeysProcessorResult(appState, "", true);
    }

    private KeysProcessorResult CreateEmptyKeysEngineResult()
    {
      return new KeysProcessorResult(_appState, "", false);
    }

  }
}

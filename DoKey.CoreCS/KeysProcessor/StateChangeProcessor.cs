using static DoKey.CoreCS.DomainUtils;

namespace DoKey.CoreCS.KeysProcessor
{
  public class StateChangeProcessor
  {
    public readonly AppState _appState;
    public readonly InputKey _inputKey;

    public StateChangeProcessor(AppState appState, InputKey inputKey)
    {
      _appState = appState;
      _inputKey = inputKey;
    }

    public AppState ProcessStateChange()
    {
      if (_appState.modificators.caps) return GetNextAppStateByStateChange();
      return GetNextAppStateByESC();
    }

    private AppState GetNextAppStateByStateChange()
    {
      var nextState = GetNextStateByKey(_appState.state, _inputKey.key);
      if (!nextState.HasValue) return null;
      return new AppState(nextState.Value, _appState.modificators, "", true);
    }

    private AppState GetNextAppStateByESC()
    {
      if (!_inputKey.isEsc) return null;
      if (_appState.state != State.Insert) return null;
      var prevState = GetPrevState(_appState.state);
      return new AppState(prevState, _appState.modificators, "", _appState.preventEscOnCapsUp);
    }

  }

}

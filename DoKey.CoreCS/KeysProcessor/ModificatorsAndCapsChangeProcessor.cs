namespace DoKey.CoreCS.KeysProcessor
{
  public class ModificatorsAndCapsChangeProcessor
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

    public KeysProcessorResult ProcessKey()
    {
      if (_inputKey.isCaps) return ProcessCapital();
      if (_inputKey.isModif) return ProcessModificators();
      return null;
    }

    private Modificators GetNextModificators()
    {
      return new ModificatorsManager(_appState.modificators, _inputKey, _isUp).GetNextModificators();
    }

    private KeysProcessorResult ProcessModificators()
    {
      var modificators = GetNextModificators();
      var nextAppState = new AppState(_appState.state, modificators, _appState.firstStep, _appState.preventEscOnCapsUp);
      return new KeysProcessorResult(nextAppState, "", false);
    }

    private string GetSendESC()
    {
      if (!_isUp) return "";
      if (_appState.preventEscOnCapsUp) return "";
      return "{ESC}";
    }

    private bool GetPreventEscOnCapsUp()
    {
      if (_isUp && _appState.preventEscOnCapsUp) return false;
      return _appState.preventEscOnCapsUp;
    }

    private KeysProcessorResult ProcessCapital()
    {
      var sendKeys = GetSendESC();
      var preventEscOnNextCapitalUp = GetPreventEscOnCapsUp();
      var modificators = GetNextModificators();
      var nextAppState = new AppState(_appState.state, modificators, "", preventEscOnNextCapitalUp);
      return new KeysProcessorResult(nextAppState, sendKeys, true);
    }

  }
}

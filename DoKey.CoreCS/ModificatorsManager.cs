namespace DoKey.CoreCS
{
  public class ModificatorsManager
  {

    private readonly Modificators _modificators;
    private readonly InputKey _inputKey;
    private readonly bool _isUp;

    public ModificatorsManager(Modificators modificators, InputKey inputKey, bool isUp)
    {
      _modificators = modificators;
      _inputKey = inputKey;
      _isUp = isUp;
    }

    public Modificators GetNextModificators()
    {
      var alt = _inputKey.isAlt ? !_isUp : _modificators.alt;
      var con = _inputKey.isControl ? !_isUp : _modificators.control;
      var sht = _inputKey.isShift ? !_isUp : _modificators.shift;
      var win = _inputKey.isWin ? !_isUp : _modificators.win;
      var cap = _inputKey.isCaps ? !_isUp : _modificators.caps;
      return new Modificators(con, sht, alt, win, cap);
    }

  }
}

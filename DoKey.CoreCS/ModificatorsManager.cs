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
      return new Modificators
      {
        alt = alt,
        control = con,
        shift = sht,
        win = win,
        caps = cap
      };
    }
    //let ModificatorsToStr(modif:Modificators) = 
    //    let str =            if modif.alt then "%" else "" 
    //                          + if modif.control then "^" else "" 
    //                          + if modif.shift then "+" else "" 
    //                          + if modif.win then "w" else "" 
    //                          + if modif.caps then "c" else "" 
    //    str

    //let ModificatorsToLog(modif:Modificators) = 
    //    "mod: " + if modif.alt then " alt " else "" 
    //                                 + if modif.control then " ctrl " else "" 
    //                                 + if modif.shift then " shift " else "" 
    //                                 + if modif.win then " win " else "" 
    //                                 + if modif.caps then " caps " else ""

  }
}

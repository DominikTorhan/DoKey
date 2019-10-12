namespace DoKey.CoreCS
{
  public class KeyEvent
  {
    public string key;
    public bool isUp;
    public override string ToString()
    {
      var s = isUp ? "Up" : "Down";
      return $"{s} {key}";
    }
  }

}

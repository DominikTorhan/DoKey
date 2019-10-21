namespace DoKey.CoreCS
{
  public class KeyEvent
  {
    public readonly string key;
    public readonly bool isUp;

    public KeyEvent(string key, bool isUp)
    {
      this.key = key;
      this.isUp = isUp;
    }

    public override string ToString()
    {
      var s = isUp ? "Up" : "Down";
      return $"{s} {key}";
    }
  }

}

using System;

namespace MemoryLeakTest.Models
{
  public class BaseItem
  {
    public BaseService Service { get; set; }

    public int KeyFromSubscriber { get; set; }

    public void EventHandler(object o, EventArgs args)
    {
      KeyFromSubscriber = ((KeyEventArgs)args).Key;
    }
  }
}

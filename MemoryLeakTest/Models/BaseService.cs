using System;

namespace MemoryLeakTest.Models
{
  public class BaseService
  {
    public event EventHandler ServiceChangedEvent;

    public BaseItem Item { get; set; }

    public void FireEvent(int key)
    {
      ServiceChangedEvent.Invoke(null, new KeyEventArgs(key));
    }
  }
}

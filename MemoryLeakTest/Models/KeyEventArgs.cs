using System;

namespace MemoryLeakTest.Models
{
  public class KeyEventArgs : EventArgs
  {
    public KeyEventArgs(int key)
    {
      Key = key;
    }

    public int Key { get; set; }
  }
}

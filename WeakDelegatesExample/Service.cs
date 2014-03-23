using System;

namespace WeakDelegatesExample
{
  // Service needn't know anything about weak references, etc. 
  // It just fires its events as normal.
  class Service
  {
    public event EventHandler MyEvent;

    public void DoIt()
    {
      if (MyEvent != null)
      {
        MyEvent(this, new EventArgs());
      }
    }

    ~Service()
    {
      Program.WriteColoredLine(" Service cleaned up", ConsoleColor.DarkGreen);
    }
  }
}

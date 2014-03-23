using System;

namespace WeakDelegatesExample
{
  // ServiceItem has a reference to Service. 
  // So that Service has a strong reference to ServiceItem. 
  class ServiceItem : IDisposable
  {
    public void EventHandler(object sender, EventArgs args)
    {
      Program.WriteColoredLine(" ServiceItem.EventHandler called", ConsoleColor.DarkRed);
    }

    private Service _service;
    public Service ServiceReference
    {
      get
      {
        return _service;
      }

      set
      {
        // subscribe new
        _service = value;
        if (_service != null)
        {
          _service.MyEvent += this.EventHandler;
        }
      }
    }

    ~ServiceItem()
    {
      Program.WriteColoredLine(" ServiceItem cleaned up", ConsoleColor.DarkGreen);
    }

    public void Dispose()
    {
      if (_service != null)
      {
        _service.MyEvent -= this.EventHandler;
        Program.WriteColoredLine(" ServiceItem disposed (removed EventHandler)", ConsoleColor.DarkGreen);
      }
    }
  }
}

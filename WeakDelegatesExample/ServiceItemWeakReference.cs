using System;

namespace WeakDelegatesExample
{
  // ServiceItemWithWeakReference has a reference to Service. But doesn't want to
  // force Service to have a strong reference to it. Uses a
  // WeakContainer private nested class for this.
  class ServiceItemWithWeakReference
  {
    public void EventHandler(object sender, EventArgs args)
    {
      Program.WriteColoredLine(" ServiceItemWithWeakReference.EventHandler called", ConsoleColor.Blue);
    }

    public Service ServiceReference
    {
      get
      {
        return _service;
      }

      set
      {
        if (_weakContainer == null)
        {
          _weakContainer = new WeakContainer(this);
        }

        // unsubscribe old
        if (_service != null)
        {
          _service.MyEvent -= _weakContainer.EventHandler;
        }

        // subscribe new
        _service = value;
        if (_service != null)
        {
          _service.MyEvent += _weakContainer.EventHandler;
        }
      }
    }

    ~ServiceItemWithWeakReference()
    {
      Program.WriteColoredLine(" ServiceItemWithWeakReference cleaned up", ConsoleColor.DarkGreen);
    }

    #region WeakContainer

    private class WeakContainer : WeakReference
    {
      public WeakContainer(ServiceItemWithWeakReference target) : base(target) { }

      public void EventHandler(object sender, EventArgs args)
      {
        Program.WriteColoredLine(" WeakContainer.EventHandler called", ConsoleColor.DarkGray);
        var b = (ServiceItemWithWeakReference)this.Target;
        if (b != null)
        {
          b.EventHandler(sender, args);
        }
        else
        {
          Service c = sender as Service;
          if (c != null)
          {
            c.MyEvent -= this.EventHandler;
            Program.WriteColoredLine(" Removed WeakContainer handler", ConsoleColor.DarkGreen);
          }
        }
      }

      ~WeakContainer()
      {
        Program.WriteColoredLine(" WeakContainer cleaned up", ConsoleColor.DarkGreen);
      }
    }

    #endregion

    private Service _service;
    private WeakContainer _weakContainer = null;
  }
}

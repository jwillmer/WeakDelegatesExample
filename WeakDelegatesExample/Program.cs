using System;

namespace WeakDelegatesExample
{
  class Program
  {
    private Service service;

    static void Main(string[] args)
    {
      (new Program()).Run();
    }

    public static void WriteColoredLine(string input, ConsoleColor color)
    {
      // only for better visualization!
      Console.BackgroundColor = color;
      Console.WriteLine(input);
      Console.ResetColor();
    }

    private void InvokeMe()
    {
      Console.WriteLine("Fire Service Event");
      if (service != null) service.DoIt();
    }

    public void Run()
    {
      ReferenceExample();

      Console.Clear();

      DisposeExample();
    }

    private void ReferenceExample()
    {
      Console.WriteLine("Initialize Service, ServiceItem & ServiceItemWithWeakReference");
      service = new Service();
      var serviceItemWithWeakReference = new ServiceItemWithWeakReference();
      var serviceItem = new ServiceItem();

      Console.WriteLine(" Setting service as reference in ServiceItem");
      Console.WriteLine(" Setting service as reference in ServiceItemWithWeakReference");
      Console.WriteLine(" Add event listener for service.MyEvent in ServiceItem ");
      Console.WriteLine(" Add event listener for service.MyEvent in ServiceItemWithWeakReference");
      serviceItemWithWeakReference.ServiceReference = service;
      serviceItem.ServiceReference = service;

      Console.WriteLine();
      InvokeMe();

      Console.WriteLine();
      Console.WriteLine("Setting ServiceItem to null and force GarbageCollecting");
      serviceItemWithWeakReference = null;
      GC.Collect();
      GC.WaitForPendingFinalizers();

      Console.WriteLine();
      InvokeMe();

      Console.WriteLine();
      Console.WriteLine("Setting serviceItem to null and force GarbageCollecting");
      // serviceItem won't removed because of the strong reference to the service
      serviceItem = null;
      GC.Collect();
      GC.WaitForPendingFinalizers();

      Console.WriteLine();
      Console.WriteLine("Setting service to null and force GarbageCollecting");
      service = null;
      GC.Collect();
      GC.WaitForPendingFinalizers();

      Console.WriteLine();
      InvokeMe();

      Console.ReadKey();
    }

    private void DisposeExample()
    {
      Console.WriteLine("Initialize Service and ServiceItem");
      service = new Service();
      var serviceItem = new ServiceItem();

      Console.WriteLine(" Setting service as reference in ServiceItem");
      Console.WriteLine(" Add event listener for service.MyEvent in ServiceItem ");
      serviceItem.ServiceReference = service;

      Console.WriteLine();
      InvokeMe();

      Console.WriteLine();
      Console.WriteLine("Calling dispose on service item");
      serviceItem.Dispose();

      Console.WriteLine();
      InvokeMe();

      Console.WriteLine();
      Console.WriteLine("Setting serviceItem to null and force GarbageCollecting");
      serviceItem = null;
      GC.Collect();
      GC.WaitForPendingFinalizers();

      Console.WriteLine();
      InvokeMe();

      Console.ReadKey();
    }
  }
}

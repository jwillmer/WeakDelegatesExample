using System;
using MemoryLeakTest.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MemoryLeakTest
{
  [TestClass]
  public class MemoryLeakTests
  {
    private BaseService service = new BaseService();
    private WeakReference weakItemReference;

    [TestInitialize]
    public void TestInitialize()
    {
      service.Item = new BaseItem();
      service.Item.Service = service;
      weakItemReference = new WeakReference(service.Item, true);
    }

    [TestMethod]
    public void CheckSubscription1Test()
    {
      int key = 001;
      service.Item.Service.ServiceChangedEvent += service.Item.EventHandler;
      service.FireEvent(key);

      Assert.AreEqual(key, service.Item.KeyFromSubscriber);
    }

    [TestMethod]
    public void CheckSubscription2Test()
    {
      int key = 002;
      service.ServiceChangedEvent += service.Item.EventHandler;
      service.FireEvent(key);

      Assert.AreEqual(key, service.Item.KeyFromSubscriber);
    }

    [TestMethod]
    public void DelegateFromItemTest()
    {
      // Event makes a strong reference
      service.Item.Service.ServiceChangedEvent += service.Item.EventHandler;
      service.Item = null;

      GC.Collect();
      GC.WaitForPendingFinalizers();

      // Memory leak!
      Assert.IsNotNull(weakItemReference.Target);
    }

    [TestMethod]
    public void DelegateFromServiceTest()
    {
      // Event makes a strong reference
      service.ServiceChangedEvent += service.Item.EventHandler;
      service.Item = null;

      GC.Collect();
      GC.WaitForPendingFinalizers();

      // Memory leak!
      Assert.IsNotNull(weakItemReference.Target);
    }

    [TestMethod]
    public void AnonymousDelegateFromItemTest()
    {
      // No strong reference with anonymous delegates
      service.Item.Service.ServiceChangedEvent += (x, y) => service.Item.EventHandler(null, y);
      service.Item = null;

      GC.Collect();
      GC.WaitForPendingFinalizers();

      Assert.IsNull(weakItemReference.Target);
    }

    [TestMethod]
    public void AnonymousDelegateFromServiceTest()
    {
      // No strong reference with anonymous delegates
      service.ServiceChangedEvent += (x, y) => service.Item.EventHandler(null, y);
      service.Item = null;

      GC.Collect();
      GC.WaitForPendingFinalizers();

      Assert.IsNull(weakItemReference.Target);
    }

    [TestMethod]
    public void RemoveDelegateTest()
    {
      service.ServiceChangedEvent += service.Item.EventHandler;
      service.ServiceChangedEvent -= service.Item.EventHandler;
      service.Item = null;

      GC.Collect();
      GC.WaitForPendingFinalizers();

      Assert.IsNull(weakItemReference.Target);
    }
  }
}

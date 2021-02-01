﻿using System.Runtime.InteropServices;
using NUnit.Framework;

namespace UJect.Tests
{
    [TestFixture]
    public class NamedAcyclicBindingTests
    {

        [Test]
        public void TestBindAcyclicGraph()
        {
            var container = new DiContainer();
            
            var frodoInstance = new Impl2("Frodo");
            var samInstance = new Impl2("Sam");
            
            var impl1 = new Impl1();
            container.Bind<IInterface1>().ToInstance(impl1);
            container.Bind<Impl1>().ToInstance(impl1);
            container.Bind<IInterface2>().WithId("Frodo").ToInstance(frodoInstance);
            container.Bind<IInterface2>().WithId("Sam").ToInstance(samInstance);
            container.Resolve();

            var fetchedImpl1 = container.GetDependency<Impl1>();
            Assert.AreEqual(frodoInstance, fetchedImpl1.Impl2Frodo);
            Assert.AreEqual(samInstance, fetchedImpl1.Impl2Sam);
        }
        
        private interface IInterface1
        {
        }
        
        private interface IInterface2
        {
        }
        
        private class Impl1 : IInterface1
        {
            [Inject("Frodo")]
            public IInterface2 Impl2Frodo;
            
            [Inject("Sam")]
            public IInterface2 Impl2Sam;
        }
        
        private class Impl2 : IInterface2
        {
            public readonly string Name;
            
            public Impl2(string name)
            {
                Name = name;
            }
        }
        
    }
}
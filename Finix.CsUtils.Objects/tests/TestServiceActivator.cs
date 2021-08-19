using System.Reflection;
using System.ComponentModel.Design;
using System;

using Xunit;

namespace Finix.CsUtils.Objects.Tests
{
    public class TestServiceActivator
    {
        private class TestService1
        {
            public bool Success { get; set; }
            public bool Called { get; set; }
        }

        private class TestService2 { }
        private class TestService3 { }

        private class TestClass1
        {
            public TestClass1(TestService1 service1, TestService2 service2, TestService3 service3) { throw new InvalidOperationException("Invalid method called!"); }
            public TestClass1(TestService1 service1, TestService3 service3) { throw new InvalidOperationException("Invalid method called!"); }
            public TestClass1(TestService1 service1, TestService2 service2) { throw new InvalidOperationException("Invalid method called!"); }

            public TestClass1(IServiceProvider serviceProvider, TestService1 service1, TestService2 service2)
            {
                if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));
                if (service1 is null) throw new ArgumentNullException(nameof(service1));
                if (service2 is null) throw new ArgumentNullException(nameof(service2));
                service1.Success = true;
            }

            public TestClass1(TestService1 service1) { throw new InvalidOperationException("Invalid method called!"); }
        }

        private class TestClass2
        {
            public void Method1(TestService1 service1) { throw new InvalidOperationException("Invalid method called!"); }

            public void Method1(TestService1 service1, TestService2 service2)
            {
                if (service1 is null) throw new ArgumentNullException(nameof(service1));
                if (service2 is null) throw new ArgumentNullException(nameof(service2));
                service1.Success = true;
            }

            public bool Method1(TestService1 service1, TestService2 service2, TestService3 service3) { throw new InvalidOperationException("Invalid method called!"); }

            public bool Method2(TestService1 service1, TestService2 service2, bool test) { throw new InvalidOperationException("Invalid method called!"); }

            public bool Method2(TestService1 service1, bool test) { throw new InvalidOperationException("Invalid method called!"); }

            public void Method2(TestService1 service1, TestService2 service2) { throw new InvalidOperationException("Invalid method called!"); }

            public bool Method2(TestService1 service1)
            {
                if (service1 is null) throw new ArgumentNullException(nameof(service1));
                return service1.Success = true;
            }

            private void Method3(IServiceProvider serviceProvider, TestService1 service1)
            {
                if (service1 is null) throw new ArgumentNullException(nameof(service1));

                if (service1.Called)
                    throw new InvalidOperationException("Invalid method called!");

                service1.Called = true;
                service1.Success = false;

                serviceProvider.Call(this, nameof(Method3), allowPrivate: true);
            }

            private void Method3(TestService1 service1)
            {
                if (service1 is null) throw new ArgumentNullException(nameof(service1));
                service1.Success = true;
            }

            private bool Method4(bool test) { throw new InvalidOperationException("Invalid method called!"); }

            private bool Method4()
            {
                return true;
            }
        }

        private IServiceProvider GetServiceProvider()
        {
            var container = new ServiceContainer();
            container.AddService(typeof(TestService1), new TestService1());
            container.AddService(typeof(TestService2), new TestService2());

            return container;
        }

        [Fact]
        public void CanCreateObject()
        {
            var services = GetServiceProvider();
            var s1 = (TestService1) services.GetService(typeof(TestService1));

            var obj = services.Create<TestClass1>();

            Assert.NotNull(obj);
            Assert.True(s1.Success);
        }

        [Fact]
        public void CanInvokePublicMethods()
        {
            var services = GetServiceProvider();
            var s1 = (TestService1) services.GetService(typeof(TestService1));

            var obj = new TestClass2();
            services.Call(obj, nameof(obj.Method1));

            Assert.True(s1.Success);

            s1.Success = false;

            MethodInfo cached = null;

            var b = services.Call<bool>(ref cached, obj, nameof(obj.Method2));

            Assert.True(b);
            Assert.True(s1.Success);

            s1.Success = false;

            b = services.Call<bool>(ref cached, obj, nameof(obj.Method2));

            Assert.True(b);
            Assert.True(s1.Success);
        }

        [Fact]
        public void CanInvokePrivateMethods()
        {
            var services = GetServiceProvider();
            var s1 = (TestService1) services.GetService(typeof(TestService1));

            var obj = new TestClass2();
            services.Call(obj, "Method3", allowPrivate: true);

            Assert.True(s1.Success);

            s1.Success = false;

            var b = services.Call<bool>(obj, "Method4", allowPrivate: true);

            Assert.True(b);
            Assert.False(s1.Success);
        }
    }
}

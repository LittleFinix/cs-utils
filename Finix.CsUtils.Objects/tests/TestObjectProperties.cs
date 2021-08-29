using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.Design;
using System;

using Xunit;

namespace Finix.CsUtils.Objects.Tests
{
    public class TestObjectProperties
    {
        public string[] TestClass1Keys = new[] {
            "Array",
            "Bar",
            "Baz",
            "Foo",
            "Number"
        };

        public string[] TestClass2Keys = new[] {
            "Class1",
            "Class1Array",
            "Class1Dict"
        };

        private class TestClass1
        {
            public string Foo { get; set; } = "Hello, World!";
            public int Bar { get; set; } = 5;

            public bool Baz { get; set; } = true;

            public float Number = 10.51f;

            public string[] Array = new[] {
                "Foo",
                "Bar",
                "Baz"
            };
        }

        private class TestClass2
        {
            public TestClass1 Class1 { get; set; } = new();

            public TestClass1[] Class1Array { get; set; } = new TestClass1[] {
                new(),
                new(),
                new()
            };

            public Dictionary<string, TestClass1> Class1Dict = new() {
                ["foo"] = new(),
                ["bar"] = new(),
                ["baz"] = new()
            };
        }

        [Fact]
        public void CanViewTestClass1()
        {
            var obj = new TestClass1();

            var view = new ObjectProperties(obj);

            var all = view.GetAll();

            Assert.Equal(TestClass1Keys, all.Keys);
            Assert.Equal(obj.Array, all["Array"]);
            Assert.Equal(obj.Bar, all["Bar"]);
            Assert.Equal(obj.Baz, all["Baz"]);
            Assert.Equal(obj.Foo, all["Foo"]);
            Assert.Equal(obj.Number, all["Number"]);

            view.SetAll(all);
            Assert.Equal(obj.Array, all["Array"]);
            Assert.Equal(obj.Bar, all["Bar"]);
            Assert.Equal(obj.Baz, all["Baz"]);
            Assert.Equal(obj.Foo, all["Foo"]);
            Assert.Equal(obj.Number, all["Number"]);

            var barProp = view.GetProperty("Bar");
            Assert.Equal(5, barProp.Value);

            barProp.Value = 10;
            Assert.Equal(10, barProp.Value);
        }

        [Fact]
        public void CanViewTestClass2()
        {
            var obj = new TestClass2();

            var view = new ObjectProperties(obj);

            var all = view.GetAll();

            Assert.Equal(TestClass2Keys, all.Keys);

            all = view.GetAll(recurse: true);

            foreach (var key in TestClass2Keys)
            {
                Assert.Contains(key, all.Keys);
            }

            foreach (var key in TestClass1Keys)
            {
                Assert.Contains("Class1." + key, all.Keys);
            }
        }
    }
}

using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.Design;
using System;

using Xunit;
using Xunit.Abstractions;
using System.Linq;

namespace Finix.CsUtils.Objects.Tests
{
    public class TestPropertySerializer
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

        public ITestOutputHelper Output { get; }

        private class TestClass1
        {
            public string Foo { get; set; }
            public int Bar { get; set; }

            public bool Baz { get; set; }

            public float Number;

            public string[] Array;

            public static TestClass1 Create()
            {
                return new() {
                    Foo = "Hello, World!",
                    Bar = 5,
                    Baz = true,
                    Number = 10.51f,
                    Array = new[] {
                        "Foo",
                        "Bar",
                        "Baz"
                    }
                };
            }
        }

        private class TestClass2
        {
            public TestClass1 Class1 { get; set; }

            public TestClass1[] Class1Array { get; set; }

            public Dictionary<string, TestClass1> Class1Dict;

            public static TestClass2 Create()
            {
                return new() {
                    Class1 = TestClass1.Create(),
                    Class1Array = new TestClass1[] {
                        TestClass1.Create(),
                        TestClass1.Create(),
                        TestClass1.Create()
                    },
                    Class1Dict = new() {
                        ["foo"] = TestClass1.Create(),
                        ["bar"] = TestClass1.Create(),
                        ["baz"] = TestClass1.Create()
                    }
                };
            }
        }

        public TestPropertySerializer(ITestOutputHelper output)
        {
            Output = output;
        }

        private static void PrintBytes(IEnumerable<byte> bytes)
        {
            const int cols = 32;
            const int group = 1;

            var i = 0;
            var g = 0;
            var chars = String.Empty;

            foreach (var b in bytes)
            {
                Debug.Write($"{b:X2}");
                chars += Char.IsControl((char) b) ? '.' : (char) b;

                if (++i % cols == 0)
                {
                    Debug.WriteLine($" | {chars}");
                    chars = String.Empty;
                    g = 0;
                }
                else if (++g % group == 0)
                {
                    Debug.Write(" ");
                }
            }

            if (!String.IsNullOrEmpty(chars))
                Debug.WriteLine($" | {chars}");
        }

        [Fact]
        public void CanBinarySerializeTestClass1()
        {
            var obj = TestClass1.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            ser.Write(obj);

            var arr = mem.ToArray();
            Assert.Equal(43, arr.Length);
            Assert.Equal(1, arr[0]);
            Assert.Equal(2, arr[1]);
        }

        [Fact]
        public void CanBinaryDeserializeTestClass1()
        {
            var obj = TestClass1.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            ser.Write(obj);

            mem.Position = 0;

            ser = new BinaryPropertySerializer(mem);

            TestClass1 tc = null;
            ser.Read(ref tc);

            Assert.Equal(obj.Foo, tc.Foo);
            Assert.Equal(obj.Bar, tc.Bar);
            Assert.Equal(obj.Baz, tc.Baz);
            Assert.Equal(obj.Number, tc.Number);
        }

        [Fact]
        public void CanBinaryMultiDeserializeTestClass1()
        {
            var obj = TestClass1.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            obj.Number = 3.14f;

            ser.Write(obj);

            mem.Position = 0;

            obj.Number = 5f;

            TestClass1 tc = null;
            ser.Read(ref tc);

            Assert.Equal(obj.Foo, tc.Foo);
            Assert.Equal(obj.Bar, tc.Bar);
            Assert.Equal(obj.Baz, tc.Baz);
            Assert.Equal(3.14f, tc.Number);

            mem.Position = 0;

            tc.Number = 15f;
            ser.Read(ref tc);

            Assert.Equal(obj.Foo, tc.Foo);
            Assert.Equal(obj.Bar, tc.Bar);
            Assert.Equal(obj.Baz, tc.Baz);
            Assert.Equal(3.14f, tc.Number);
        }

        [Fact]
        public void CanBinarySerializeTestClass2()
        {
            var obj = TestClass2.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            ser.Write(obj);

            var arr = mem.ToArray();

            PrintBytes(arr);

            Assert.Equal(326, arr.Length);
            Assert.Equal(1, arr[0]);
            Assert.Equal(2, arr[1]);

        }

        [Fact]
        public void CanBinaryDeserializeTestClass2()
        {
            var obj = TestClass2.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            ser.Write(obj);

            PrintBytes(mem.ToArray());
            mem.Position = 0;

            ser = new BinaryPropertySerializer(mem);

            TestClass2 tc = null;
            ser.Read(ref tc);

            Assert.Equal(obj.Class1.Foo, tc.Class1.Foo);
            Assert.Equal(obj.Class1.Bar, tc.Class1.Bar);
            Assert.Equal(obj.Class1.Baz, tc.Class1.Baz);
            Assert.Equal(obj.Class1.Number, tc.Class1.Number);

            Assert.Equal(3, tc.Class1Dict.Keys.Count);
            Assert.Equal(new[] { "foo", "bar", "baz" }, tc.Class1Dict.Keys);
        }

        [Fact]
        public void CanBinaryMultiDeserializeTestClass2()
        {
            var obj = TestClass2.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            obj.Class1.Number = 3.14f;

            ser.Write(obj);

            mem.Position = 0;

            obj.Class1.Number = 5f;

            TestClass2 tc = null;
            ser.Read(ref tc);

            Assert.Equal(3.14f, tc.Class1.Number);

            mem.Position = 0;
            tc.Class1.Number = 9f;
            ser.Write(tc);

            mem.Position = 0;

            tc.Class1.Number = 15f;
            ser.Read(ref tc);

            Assert.Equal(9f, tc.Class1.Number);
        }

        [Fact]
        public void CanBinaryDeserializeAndKeepReference()
        {
            var obj = TestClass2.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);
            var des = new BinaryPropertySerializer(mem);

            obj.Class1.Number = 3.14f;

            ser.Write(obj);

            TestClass2 tc = null;
            mem.Position = 0;
            des.Read(ref tc);

            var c1 = tc.Class1;
            c1.Number = 4;

            mem.Position = 0;
            des.Read(ref tc);

            Assert.Same(c1, tc.Class1);

            var c2 = obj.Class1Array[0];
            var c3 = obj.Class1Array[1];

            Assert.NotNull(c2);
            Assert.NotNull(c3);

            c2.Number = 2;
            c3.Number = 3;

            obj.Class1Array[0] = c3;
            obj.Class1Array[1] = c2;

            c2 = tc.Class1Array[0];
            c3 = tc.Class1Array[1];

            mem.Position = 0;
            ser.Write(obj);

            mem.Position = 0;
            des.Read(ref tc);

            Assert.Same(c3, tc.Class1Array[0]);
            Assert.Same(c2, tc.Class1Array[1]);

            Assert.Equal(2, c2.Number);
            Assert.Equal(3, c3.Number);
        }

        [Fact]
        public void CanBinaryRoundtripWithinTime()
        {
            var obj = TestClass2.Create();
            var mem = new MemoryStream();
            var ser = new BinaryPropertySerializer(mem);

            var sw = new Stopwatch();
            int times;

            for (times = 0; times < 100; times++)
            {
                mem.Position = 0;
                ser.Write(obj);
                mem.Position = 0;
                ser.Read(ref obj);
                Assert.Equal(10.51f, obj.Class1.Number);
            }

            Output.WriteLine($"warmup done");

            sw.Start();

            for (times = 0; times < 10000; times++)
            {
                mem.Position = 0;
                ser.Write(obj);
                mem.Position = 0;
                ser.Read(ref obj);
            }

            sw.Stop();

            var avg = sw.ElapsedMilliseconds / (decimal) times;
            Output.WriteLine($"{times} roundtrips: {avg}ms/rt");

            Assert.True(avg < 3);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Ingi;
using NUnit.Framework;

namespace InstanceCounterTests
{
    [TestFixture]
    public class InstanceCounterTests
    {
        [Test]
        public void Traverse_Null_Empty()
        {
            var retVal = InstanceCounter.Traverse(null);
            Assert.IsFalse(retVal.Any());
        }

        [Test]
        [TestCase(1)]
        [TestCase(7)]
        [TestCase(53)]
        public void Traverse_List_CorrectCount(int count)
        {
            IList<int> list = new List<int>();
            for (int i = 0; i < count; i++)
                list.Add(i);
            var retVal = InstanceCounter.Traverse(list);
            Assert.IsTrue(retVal[typeof(int)] == count);
        }

        [Test]
        [TestCase(3)]
        [TestCase(11)]
        [TestCase(12)]
        public void Traverse_Array_CorrectCount(int count)
        {
            int[] list = new int[count];
            for (int i = 0; i < count; i++)
                list[i] = i;
            var retVal = InstanceCounter.Traverse(list);
            Assert.IsTrue(retVal[typeof(int)] == count);
        }

        [Test]
        public void Traverse_Int_CorrectType()
        {
            const int number = 1234;
            var retVal = InstanceCounter.Traverse(number);
            Assert.IsTrue(retVal.Single().Key == number.GetType());
        }

        [Test]
        public void Traverse_A_FourA()
        {
            A a = new A()
            {
                One = new A()
                {
                    Many = new List<A>(2) { new A(), new A()}
                }
            };

            var retVal = InstanceCounter.Traverse(a);
            int aCount = retVal[typeof(A)];
            Assert.IsTrue(aCount == 4);
        }

        [Test]
        public void Traverse_ListOfLists_FourB()
        {
            B b = new B()
            {
                ListOfLists = new List<List<B>>()
                {
                    new List<B>() { new B(), new B(), new B()}
                }
            };

            var retVal = InstanceCounter.Traverse(b);
            int aCount = retVal[typeof(B)];
            Assert.IsTrue(aCount == 4);
        }

        class A
        {
            public A One { get; set; }
            public IEnumerable<A> Many { get; set; }
        }

        class B
        {
            public List<List<B>> ListOfLists { get; set; }
        }
    }
}
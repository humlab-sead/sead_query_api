using Autofac;
using Xunit;
using SeadQueryCore;
using SeadQueryInfra;
using SQT.Infrastructure;
using SQT.Mocks;
using System.Collections.Generic;
using System;

namespace SQT.Infrastructure
{
    public class UtilityTests
    {
        [Fact]
        public void InsertAt_EmptyList_ThrowsException()
        {
            var items = new List<string>();
            const string item = "A";
            Assert.Throws<ArgumentException>(() => items.InsertAt(itemToFind: "B", itemToInsert: item));
        }

        [Fact]
        public void InsertAt_SingleItem_InsertsFirst()
        {
            var items = new List<string>() { "B" };
            items.InsertAt(itemToFind: "B", itemToInsert: "A");
            Assert.Equal(new List<string>() { "A", "B" }, items);
        }

        [Fact]
        public void InsertAt_Item_IsOk()
        {
            var items = new List<string>() { "A", "B", "C", "E", "F" };
            items.InsertAt(itemToFind: "E", itemToInsert: "D");
            Assert.Equal(new List<string>() { "A", "B", "C", "D", "E", "F" }, items);
        }
    }
}

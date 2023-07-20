using System.Collections.Generic;
using NUnit.Framework;
using Super_Auto_Mobs;
using UnityEngine;

namespace UnitTests
{
    public class EditorTests
    {
        [Test]
        public void Vector_1_0_Vector_1_0return()
        {
            Assert.AreEqual(new Vector2(0, 0).SetX(1), new Vector2(1, 0));
        }
        
        [Test]
        public void RandomExtensions_2_GetRandomList_4_return()
        {
            var list = new List<int>() {1, 2};
            var newList = RandomExtensions.GetRandomList(list, 4);
            Assert.AreEqual(newList.Count, 4);
        }
        
        [Test]
        public void RandomExtensions_4_GetRandomList_4_return()
        {
            var list = new List<int>() {1, 2, 3, 4};
            var newList = RandomExtensions.GetRandomList(list, 4);
            Assert.AreEqual(list.Count, newList.Count);
        }
    }
}

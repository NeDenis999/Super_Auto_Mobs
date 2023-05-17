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
    }
}

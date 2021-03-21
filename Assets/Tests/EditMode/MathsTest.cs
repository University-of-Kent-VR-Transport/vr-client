using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	public class MathsTest
	{
		[Test]
		public void AdditionTest()
		{
			Assert.AreEqual(7, Addition.run(5, 2));
		} 
	}
}

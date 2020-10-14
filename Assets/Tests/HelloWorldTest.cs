using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	public class HelloWorldTest
	{
		[Test]
		public void testAdd()
		{
			var helloWorld = new HelloWorld();
			
			var output = helloWorld.add(1, 2);
			Assert.AreEqual(3, output);
		}
	}
}

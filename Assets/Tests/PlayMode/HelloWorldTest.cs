using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
	public class HelloWorldTest
	{
		[UnityTest]
		public IEnumerator testAdd()
		{
			var gameObject = new GameObject();
			var helloWorld = gameObject.AddComponent<HelloWorld>();
			
			var output = helloWorld.add(1, 2);
			
			yield return null;

			Assert.AreEqual(3, output);
		}
	}
}

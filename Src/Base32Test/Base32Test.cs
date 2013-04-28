﻿using Pug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Base32Test
{
	
	
	/// <summary>
	///This is a test class for Base32Test and is intended
	///to contain all Base32Test Unit Tests
	///</summary>
	[TestClass()]
	public class Base32Test
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for From
		///</summary>
		[TestMethod()]
		public void FromTest()
		{
			byte[] data = new byte[] {65, 16, 4, 135, 170, 26}; // TODO: Initialize to an appropriate value
			string expected = "ABDHPCJU"; // TODO: Initialize to an appropriate value
			string actual;
			actual = Base32.From(data);

			//byte[] returnData = Base32.GetBytes(actual);

			//Assert.AreEqual(data, returnData);
			//Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}

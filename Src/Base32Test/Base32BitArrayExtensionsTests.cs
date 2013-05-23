using Pug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Base32Test
{
    
    
    /// <summary>
    ///This is a test class for Base32BitArrayExtensionsTests and is intended
    ///to contain all Base32BitArrayExtensionsTests Unit Tests
    ///</summary>
	[TestClass()]
	public class Base32BitArrayExtensionsTests
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
		///A test for Encode
		///</summary>
		[TestMethod()]
		public void EncodeTest()
		{
			byte[] data = new byte[] {70, 1}; // TODO: Initialize to an appropriate value
			string expected = "GKAA"; // TODO: Initialize to an appropriate value
			string actual;
			actual = Base32.Encode(data);
			Assert.AreEqual(expected, actual);
		}
	}
}

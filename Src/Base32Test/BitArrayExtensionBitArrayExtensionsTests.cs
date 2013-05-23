using Pug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;

namespace Base32Test
{
    
    
    /// <summary>
    ///This is a test class for BitArrayExtensionBitArrayExtensionsTests and is intended
    ///to contain all BitArrayExtensionBitArrayExtensionsTests Unit Tests
    ///</summary>
	[TestClass()]
	public class BitArrayExtensionBitArrayExtensionsTests
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
		///A test for GetBytes
		///</summary>
		[TestMethod()]
		public void GetBytesTest()
		{
			BitArray bitArray = new BitArray(new byte[] {70, 1}); // TODO: Initialize to an appropriate value
			int start = 6; // TODO: Initialize to an appropriate value
			int length = 5; // TODO: Initialize to an appropriate value
			byte[] expected = new byte[] {17}; // TODO: Initialize to an appropriate value
			byte[] actual;
			actual = bitArray.GetBytes(start, length);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}

using Pug.Application.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CsvTests
{
    
    
    /// <summary>
    ///This is a test class for CsvFileTest and is intended
    ///to contain all CsvFileTest Unit Tests
    ///</summary>
	[TestClass()]
	public class CsvFileTest
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
		///A test for ReadNext
		///</summary>
		[TestMethod()]
		public void ReadNextTest()
		{
			string file = @"F:\Pug Computing Services\Development\PugFX\1.0\Src\Tests\CsvTests\startrackexportBAYSWATER(4).din"; // TODO: Initialize to an appropriate value
			CsvReader target = CsvReader.Open(file); // TODO: Initialize to an appropriate value
			CsvReader.CsvLine expected = null; // TODO: Initialize to an appropriate value
			CsvReader.CsvLine actual;
			actual = target.ReadLine();

			while (actual != null && actual.Length > 0)
			{
				actual = target.ReadLine();
			}

			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}

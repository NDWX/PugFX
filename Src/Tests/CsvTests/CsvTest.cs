using Pug.Application.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CsvTests
{
    
    
    /// <summary>
    ///This is a test class for CsvTest and is intended
    ///to contain all CsvTest Unit Tests
    ///</summary>
	[TestClass()]
	public class CsvTest
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
		///A test for ReadFromFile
		///</summary>
		[TestMethod()]
		public void ReadFromFileTest()
		{
			//string path = @"F:\Pug Computing Services\Development\PugFX\1.0\Src\Tests\CsvTests\testdata.csv"; // TODO: Initialize to an appropriate value
			//char separator = '"'; // TODO: Initialize to an appropriate value
			//Csv expected = null; // TODO: Initialize to an appropriate value
			//Csv actual;
			//actual = Csv.ReadFromFile(path, separator);
			//Assert.AreEqual(expected, actual);
			//Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}

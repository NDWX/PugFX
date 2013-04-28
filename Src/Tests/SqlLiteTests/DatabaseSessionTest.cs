using Pug.Application.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;

namespace SqlLiteTests
{
    
    
    /// <summary>
    ///This is a test class for DatabaseSessionTest and is intended
    ///to contain all DatabaseSessionTest Unit Tests
    ///</summary>
	[TestClass()]
	public class DatabaseSessionTest
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
		///A test for Execute
		///</summary>
		[TestMethod()]
		public void ExecuteTest()
		{
			IDataAccessProvider dataAccessProvider = new Pug.Application.Data.SqlLite.DataAccessProvider();

			DbConnection connection = dataAccessProvider.DbProviderFactory.CreateConnection(); // TODO: Initialize to an appropriate value
			connection.ConnectionString = @"Data Source=I:/pApplications/SqlLite/aup-parcel.plp";
			DataExceptionHandler exceptionHandler = null; // TODO: Initialize to an appropriate value
			connection.Open();
			DatabaseSession target = new DatabaseSession(connection, exceptionHandler); // TODO: Initialize to an appropriate value
			DbCommand command = dataAccessProvider.DbProviderFactory.CreateCommand(); // TODO: Initialize to an appropriate value
			command.CommandText = "select * from zone";
			int expected = 0; // TODO: Initialize to an appropriate value
			DbDataReader actual;
			actual = target.ExecuteQuery(command);
			Assert.AreEqual(expected, actual);
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}

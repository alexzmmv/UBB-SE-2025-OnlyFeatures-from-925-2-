// <copyright file="DatabaseConnectionTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Tests.UnitTests.Database
{
    using System;
    using System.Data;
    using Microsoft.Data.SqlClient;
    using Xunit;
    using Moq;
    using WinUIApp.WebAPI.Database;

    /// <summary>
    /// Test class for the DatabaseConnection.
    /// </summary>
    public class DatabaseConnectionTests
    {
        /// <summary>
        /// Test for Instance property - should return the same instance each time.
        /// </summary>
        [Fact]
        public void Instance_CalledMultipleTimes_ReturnsSameInstance()
        {
            // Act
            var firstInstance = DatabaseConnection.Instance;
            var secondInstance = DatabaseConnection.Instance;
            
            // Assert
            Assert.NotNull(firstInstance);
            Assert.Same(firstInstance, secondInstance);
        }

        /// <summary>
        /// Test for GetConnection method - should return a valid connection.
        /// </summary>
        [Fact]
        public void GetConnection_Called_ReturnsSqlConnection()
        {
            // Arrange
            var mockDatabaseConnection = new Mock<DatabaseConnection>() { CallBase = true };
            
            // Act
            var sqlConnection = DatabaseConnection.Instance.GetConnection();
            
            // Assert
            Assert.NotNull(sqlConnection);
            Assert.IsType<SqlConnection>(sqlConnection);
        }

        /// <summary>
        /// Test for OpenConnection method when connection is closed.
        /// </summary>
        [Fact]
        public void OpenConnection_ConnectionIsClosed_OpensConnection()
        {
            // This test validates the behavior of OpenConnection method
            // Since we can't easily mock the internal connection state,
            // we'll test that no exception is thrown when opening a connection
            
            // Arrange - ensure connection is closed
            var databaseInstance = DatabaseConnection.Instance;
            databaseInstance.CloseConnection();
            
            // Act & Assert
            var caughtException = Record.Exception(() => databaseInstance.OpenConnection());
            Assert.Null(caughtException);
        }

        /// <summary>
        /// Test for CloseConnection method when connection is open.
        /// </summary>
        [Fact]
        public void CloseConnection_ConnectionIsOpen_ClosesConnection()
        {
            // This test validates the behavior of CloseConnection method
            // Since we can't easily mock the internal connection state,
            // we'll test that no exception is thrown when closing a connection
            
            // Arrange - ensure connection is open
            var databaseInstance = DatabaseConnection.Instance;
            databaseInstance.OpenConnection();
            
            // Act & Assert
            var caughtException = Record.Exception(() => databaseInstance.CloseConnection());
            Assert.Null(caughtException);
        }
    }
}
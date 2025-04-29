// <copyright file="DatabaseConnection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Database
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Singleton class for managing the database connection.
    /// </summary>
    public class DatabaseConnection
    {
        private static readonly object @Lock = new ();
        private static readonly string ConnectionString;
        private static DatabaseConnection instance;
        private SqlConnection connection;

        static DatabaseConnection()
        {
            // Initialize configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Set base path to current directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // Load the appsettings file
                .Build();

            // Retrieve the connection string from appsettings.json
            ConnectionString = configuration.GetValue<string>("DbConnection");
        }

        /// <summary>
        /// Gets the singleton instance of the DatabaseConnection class.
        /// </summary>
        public static DatabaseConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (@Lock)
                    {
                        instance ??= new DatabaseConnection();
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the connection to the database.
        /// </summary>
        /// <returns> An sql connection. </returns>
        public SqlConnection GetConnection()
        {
            if (this.connection == null || this.connection.State == System.Data.ConnectionState.Broken)
            {
                this.connection = new SqlConnection(ConnectionString);
            }

            return this.connection;
        }

        /// <summary>
        /// Opens the connection to the database.
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                var connection = this.GetConnection();

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error opening connection: {exception.Message}");
            }
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                var connection = this.GetConnection();

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error closing connection: {exception.Message}");
            }
        }
    }
}

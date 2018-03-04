//  
// Copyright (c) AutoIt Consulting Ltd. All rights reserved.  
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.  
//

using System;
using System.Net;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

namespace AutoIt.SCCM.SDK.Snippets
{
    /// <summary>
    /// Wrapper class for running SCCM SDK snippets.
    /// </summary>
    internal partial class Wrapper
    {
        /// <summary>
        ///     The main entry point for the wrapper. Uncomment the snippets you wish to run here.
        /// </summary>
        public void Run()
        {
            var computer = "";
            var userName = "";
            var password = "";

            Console.WriteLine("Site server you want to connect to (press Return for this computer): ");
            computer = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrEmpty(computer) || computer == ".")
            {
                computer = Dns.GetHostName();
                userName = "";
                password = "";
            }
            else
            {
                Console.WriteLine("Please enter the user name (press Return for current user): ");
                userName = Console.ReadLine();

                if (!string.IsNullOrEmpty(userName))
                {
                    Console.WriteLine("Please enter your password: ");
                    password = ReturnPassword();
                }
            }

            // Make connection to provider.
            WqlConnectionManager wqlConnection = Connect(computer, userName, password);
            if (wqlConnection == null)
            {
                return;
            }

            //
            // Call snippets - uncomment required snippets.
            //

            //
            // Fundamentals
            //
            //IResultObjectExecuteQueryDisposalV1(wqlConnection);
            //IResultObjectExecuteQueryDisposalV2(wqlConnection);
            //IResultObjectDisposalOfReturnValue(wqlConnection);
            LazyPropertyFromQuery(wqlConnection);

            // Disconnect
            wqlConnection.Close();
            wqlConnection.Dispose();
        }

        /// <summary>
        /// Connects the the SCCM site server.
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns>WqlConnectionManager object or null if failed.</returns>
        public WqlConnectionManager Connect(string serverName, string userName, string userPassword)
        {
            try
            {
                Console.WriteLine("Connecting...");
                var namedValues = new SmsNamedValuesDictionary();
                var connection = new WqlConnectionManager(namedValues);
                if (string.IsNullOrEmpty(userName))
                {
                    connection.Connect(serverName);
                }
                else
                {
                    connection.Connect(serverName, userName, userPassword);
                }

                return connection;
            }
            catch (SmsException ex)
            {
                Console.WriteLine("Failed to Connect. Error: " + ex.Message);
                return null;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Failed to authenticate. Error:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Allows the user to type in a hidden password.
        /// </summary>
        /// <returns></returns>
        public string ReturnPassword()
        {
            var password = "";

            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    password += info.KeyChar;
                    info = Console.ReadKey(true);
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                    }

                    info = Console.ReadKey(true);
                }
            }

            for (var i = 0; i < password.Length; i++)
            {
                Console.Write("*");
            }

            return password;

        }
    }
}
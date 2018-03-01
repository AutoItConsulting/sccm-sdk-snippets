//  
// Copyright (c) AutoIt Consulting Ltd. All rights reserved.  
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.  
//

using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace AutoIt.SCCM.SDK.Snippets  
{
    internal class Program  
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)  
        {  
            // Catch assembly references that can't be resolved - then load them from the CM Admin Console folder
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += OnResolveAssembly;

            // Setup snippet class.  
            var wrapper = new Wrapper();
            wrapper.Run();
        }
        
        /// <summary>
        /// Called when assemblies cannot be resolved. For SCCM related assemblies try and resolve from the Admin Console folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly OnResolveAssembly(object sender, ResolveEventArgs args)
        {
            // Only concern ourself with CM related DLLs
            if (!args.Name.StartsWith("AdminUI.") && !args.Name.StartsWith("DcmObjectModel") && !args.Name.StartsWith("Microsoft.ConfigurationManagement."))
            {
                return null;
            }

            var askedAssemblyName = new AssemblyName(args.Name);

            // failing to ignore queries for satellite resource assemblies or using [assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)] 
            // in AssemblyInfo.cs will crash the program on non en-US based system cultures.
            if (askedAssemblyName.Name.EndsWith(".resources") && askedAssemblyName.CultureInfo.Equals(CultureInfo.InvariantCulture) == false)
            {
                return null;
            }

            string assemblyDllName = askedAssemblyName.Name;

            assemblyDllName += ".dll";

            string consolePath = Environment.GetEnvironmentVariable("SMS_ADMIN_UI_PATH");

            string message = "The following DLL is missing:\r\n" + assemblyDllName;
            message += "\r\n\r\n";
            message += "Please install the Configuration Manager Admin Console or copy the named DLL into the application directory.";

            string assemblyDllPath = string.Empty;

            if (!string.IsNullOrEmpty(consolePath))
            {
                DirectoryInfo directoryInfo = Directory.GetParent(consolePath);
                assemblyDllPath = directoryInfo.FullName;
            }

            assemblyDllPath += "\\" + assemblyDllName;

            if (File.Exists(assemblyDllPath))
            {
                Assembly assembly = Assembly.LoadFrom(assemblyDllPath);
                return assembly;
            }

            //MessageBox.Show(message);
            Console.WriteLine(message);

            Environment.Exit(1);
            return null;
        }
    }  
}
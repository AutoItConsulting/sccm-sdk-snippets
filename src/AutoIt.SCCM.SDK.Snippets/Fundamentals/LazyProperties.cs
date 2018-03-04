//  
// Copyright (c) AutoIt Consulting Ltd. All rights reserved.  
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.  
//

using System;
using System.Collections.Generic;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

namespace AutoIt.SCCM.SDK.Snippets
{
    internal partial class Wrapper
    {
        public void LazyPropertyFromQuery(WqlConnectionManager connection)  
        {  
            try  
            {  
                // Query the All Systems collection (this has two default rules)
                using (IResultObject collections = connection.QueryProcessor.ExecuteQuery("Select * from SMS_Collection Where CollectionID='SMS00001'"))
                {
                    foreach (IResultObject collection in collections)
                    {
                        // Show collection name
                        Console.WriteLine(collection["Name"].StringValue);

                        // Get the collection object and lazy properties.  
                        collection.Get();

                        // Get the rules from CollectionRules which is a lazy property  
                        List<IResultObject> rules = collection.GetArrayItems("CollectionRules");
                        
                        if (rules.Count == 0)
                        {
                            Console.WriteLine("No rules");
                            continue;
                        }

                        foreach (IResultObject rule in rules)
                        {
                            // Display rule names.  
                            Console.WriteLine("Rule name: " + rule["RuleName"].StringValue);
                        }
                    }
                }
            }  
            catch (SmsQueryException ex)  
            {  
                Console.WriteLine("Failed to get collection. Error: " + ex.Message);  
                throw;  
            }  
        } 
    }
}

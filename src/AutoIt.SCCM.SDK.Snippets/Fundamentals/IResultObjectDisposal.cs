//  
// Copyright (c) AutoIt Consulting Ltd. All rights reserved.  
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.  
//

using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

namespace AutoIt.SCCM.SDK.Snippets
{
    internal partial class Wrapper
    {
        /// <summary>
        /// IResultObject disposal version 1. Automatic Dispose with Using keyword.
        /// </summary>
        /// <param name="wqlConnection"></param>
        private void IResultObjectDisposalV1(WqlConnectionManager wqlConnection)
        {
            try
            {
                // As an example, get all SMS_Package objects.
                using (IResultObject queryResults = wqlConnection.QueryProcessor.ExecuteQuery("SELECT * FROM SMS_Package"))
                {
                    foreach (IResultObject item in queryResults)
                    {
                        // Must call Dispose on each item enumerated
                        using (item)
                        {
                            string packageName = item["Name"].StringValue;
                            OutputLine("Package: " + packageName);
                        }
                    }
                }
            }
            catch (SmsException)
            {
                // Error handling.
            }
        }

        /// <summary>
        /// IResultObject disposal version 2. Manual call of Dipose.
        /// </summary>
        /// <param name="wqlConnection"></param>
        private void IResultObjectDisposalV2(WqlConnectionManager wqlConnection)
        {
            try
            {
                // As an example, get all SMS_Package objects.
                using (IResultObject queryResults = wqlConnection.QueryProcessor.ExecuteQuery("SELECT * FROM SMS_Package"))
                {
                    foreach (IResultObject item in queryResults)
                    {
                        string packageName = item["Name"].StringValue;
                        OutputLine("Package: " + packageName);

                        // Must call Dispose on each item enumerated
                        item.Dispose();
                    }
                }
            }
            catch (SmsException)
            {
                // Error handling.
            }
        }

        /// <summary>
        /// Dispose an IResultObject returned from another method.
        /// </summary>
        /// <param name="wqlConnection"></param>
        private void IResultObjectDisposalOfReturnValue(WqlConnectionManager wqlConnection)
        {
            // Call method that returns an IResultObject - we are responsible for calling Dispose.
            using (IResultObject item = IResultObjectReturnSmsPackage(wqlConnection))
            {
                string packageName = item["Name"].StringValue;
                OutputLine("Package: " + packageName);
            }
        }

        /// <summary>
        /// Return an IResultObject.
        /// </summary>
        /// <param name="wqlConnection"></param>
        private IResultObject IResultObjectReturnSmsPackage(WqlConnectionManager wqlConnection)
        {
            try
            {
                // As an example, get all SMS_Package objects.
                using (IResultObject queryResults = wqlConnection.QueryProcessor.ExecuteQuery("SELECT * FROM SMS_Package"))
                {
                    // Just return the first item as an example
                    foreach (IResultObject item in queryResults)
                    {
                        // Don't call Dispose on the item, the caller will now be responsible for it
                        return item;
                    }
                }
            }
            catch (SmsException)
            {
                // Error handling.
                return null;
            }

            // No items
            return null;
        }
    }
}
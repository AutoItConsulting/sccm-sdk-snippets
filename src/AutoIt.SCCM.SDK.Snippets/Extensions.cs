//  
// Copyright (c) AutoIt Consulting Ltd. All rights reserved.  
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.  
//

using System.Collections.Generic;
using Microsoft.ConfigurationManagement.ManagementProvider;

namespace AutoIt.SCCM.SDK.Snippets
{
    public static class Extensions
    {
        /// <summary>
        ///     Runs Dispose() on all IResultObject items in an IEnumerable container such as a List. Use this when you have a list
        ///     of IResultObjects and you want to Dipose() each to release WMI resources.
        /// </summary>
        public static void DisposeItems(this IEnumerable<IResultObject> items)
        {
            foreach (IResultObject item in items)
            {
                item.Dispose();
            }
        }
    }
}

﻿using System.Globalization;

namespace FashionNest.Infrastructure.VnPay.DependencyInjection.Options.Lib
{
    public class VnpayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}

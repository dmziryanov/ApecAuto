using System;

namespace RmsAuto.TechDoc
{
    public static class Utils
    {
        public static string ParseIntToDT(int? val)
        {
            if (val.HasValue)
            {
                string strVal = val.ToString();
                if (strVal.Length == 6)
                {
                    return String.Format("{1}/{0}", strVal.Substring(0, 4), strVal.Substring(4));
                }
            }
            return String.Empty;
        }
    }
}

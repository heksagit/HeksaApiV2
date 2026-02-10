using System;
using System.Collections.Generic;
using System.Text;

namespace HeksaApiV2.Common.Helpers
{
    public static class SpajHelper
    {
        public static bool IsValidBMI(int height, int weight)
        {
            float heightinMeter = height / 100.0f;
            double _BMI = weight / Math.Pow(heightinMeter, 2);
            if (_BMI < 16 || _BMI > 35)
                return false;
            else
                return true;
        } 
    }
}

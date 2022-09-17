namespace Extensions
{
    public class UIExtensions
    {
        public static string GetAdaptedValue(int value, string tagPrefixes = "")
        {
            if (value > 10000000)
            {
                int thousands = value / 1000000;
                return $"{thousands}{tagPrefixes}>kk";
            }
            
            if (value > 100000)
            {
                int thousands = value / 1000;
                return $"{thousands}{tagPrefixes}k";
            }

            return value.ToString();
        }
    }
}
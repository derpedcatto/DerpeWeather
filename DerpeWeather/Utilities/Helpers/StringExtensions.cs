using System.Text;

namespace DerpeWeather.Utilities.Helpers
{
    public static class StringExtensions
    {
        public static string TrimNewLines(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            StringBuilder sb = new(source);

            // Remove '\n' characters at the start
            while (sb.Length > 0 && sb[0] == '\n')
            {
                sb.Remove(0, 1);
            }

            // Remove '\n' characters at the end
            while (sb.Length > 0 && sb[sb.Length - 1] == '\n')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}

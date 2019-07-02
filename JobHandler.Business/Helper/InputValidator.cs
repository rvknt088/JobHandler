using System.Text.RegularExpressions;

namespace JobHandler.Business.Helper
{
    public class InputValidator
    {
        #region [Validate Input]
        /// <summary>
        /// Validate Input
        /// Example
        /// a =>
        /// b => c
        /// c =>
        /// </summary>
        /// <param name="input">string input</param>
        /// <returns>Success</returns>
        public static bool ValidateInput(string input)
        {
            var regex = @"(^[a-z]{1}\s{1}?)(=>)(|(\s{1}?[a-z]{1}?))";
            var match = Regex.Match(input, regex, RegexOptions.IgnoreCase);
            return match.Success;
        }
        #endregion [Validate Input]
    }
}

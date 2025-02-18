namespace AwesomeBank.API.Application.Validations
{
    public static class CommonValdations
    {
        public static bool IsValidDecimalPrecision(decimal value, int precision)
        {
            // Check if the decimal value has more than two decimal places
            return value == Math.Round(value, precision);
        }

        public static bool IsValidNumber(string value)
        {
            // Check if the decimal value has more than two decimal places
            return int.TryParse(value, out int number);
        }

        public static bool IsValidDate(DateTime date)
        {
            // Ensure the date is not the default date (DateTime.MinValue) or the maximum date (DateTime.MaxValue)
            return date > DateTime.MinValue && date < DateTime.MaxValue;
        }
    }
}
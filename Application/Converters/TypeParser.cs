using System.Globalization;

namespace Application.Converters;

public static class TypeParser
{
    private const string MB_Name = "mb";
    private const string GB_Name = "gb";
    private const string TB_Name = "tb";

    public static int? ParseHardDiskOrRam(string value)
    {
        // Extract the numeric part from the string
        string numericPart = new string(value.Where(char.IsDigit).ToArray());

        // Check for unit (GB, MB, TB) and adjust the value accordingly
        if (value.ToLower().Contains(TB_Name) || value.Count() == 1)
        {
            // Convert TB to GB (1 TB = 1000 GB)
            if (int.TryParse(numericPart, out int result))
            {
                return result * 1000;
            }
        }
        else if (value.ToLower().Contains(MB_Name))
        {
            // Convert MB to GB (1 MB = 0.001 GB)
            if (int.TryParse(numericPart, out int result))
            {
                return (int)(result * 0.001);
            }
        }
        else if (value.ToLower().Contains(GB_Name) || value.Count() > 0)
        {
            // No conversion needed for GB
            if (int.TryParse(numericPart, out int result))
            {
                return result;
            }
        }

        // Parsing failed - return null
        return null;
    }

    public static double? ParseToDouble(string value)
    {
        // Remove any non-numeric characters except for decimal point
        string cleanedValue = new string(value.Where(c => char.IsDigit(c) || c == '.').ToArray());

        // Try parsing the cleaned value to double
        if (double.TryParse(cleanedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }

        // Parsing failed - return null
        return null;
    }
}

using System.Globalization;

namespace ProductsUiBlazor.Data.Utilities;

public static class FileTypeDetector
{
    public static bool IsPdf(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".pdf")
        {
            return true;
        }
        return false;
    }
    public static bool IsImage(string filename)
    {
        if (IsBmp(filename) || IsPng(filename) || IsJpg(filename) || IsGif(filename) || IsSvg(filename) || IsTiff(filename))
        {
            return true;
        }
        return false;
    }

    public static bool IsTiff(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".tif" || Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".tiff")
        {
            return true;
        }
        return false;
    }

    public static bool IsBmp(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".bmp")
        {
            return true;
        }
        return false;
    }

    public static bool IsGif(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".gif")
        {
            return true;
        }
        return false;
    }

    public static bool IsPng(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".png")
        {
            return true;
        }
        return false;
    }

    public static bool IsJpg(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".jpg")
        {
            return true;
        }
        return false;
    }

    public static bool IsSvg(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".svg")
        {
            return true;
        }
        return false;
    }

    public static bool IsOpenXml(string filename)
    {
        if (IsWord(filename) || IsExcel(filename) || IsPowerpoint(filename))
        {
            return true;
        }
        return false;
    }

    public static bool IsWord(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".doc" || Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".docx")
        {
            return true;
        }
        return false;
    }

    public static bool IsExcel(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".xls" || Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".xlsx")
        {
            return true;
        }
        return false;
    }

    public static bool IsPowerpoint(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".ppt" || Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".pptx")
        {
            return true;
        }
        return false;
    }

    public static bool IsEmail(string filename)
    {
        if (IsMsg(filename) || IsEml(filename))
        {
            return true;
        }
        return false;
    }

    public static bool IsMsg(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".msg")
        {
            return true;
        }
        return false;
    }

    public static bool IsEml(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".eml")
        {
            return true;
        }
        return false;
    }

    public static bool IsText(string filename)
    {
        if (IsTxt(filename) || IsBat(filename) || IsLog(filename) || IsCsv(filename) || IsXml(filename) || IsJson(filename))
        {
            return true;
        }
        return false;
    }

    public static bool IsTxt(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".txt")
        {
            return true;
        }
        return false;
    }
    public static bool IsBat(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".bat")
        {
            return true;
        }
        return false;
    }

    public static bool IsLog(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".log")
        {
            return true;
        }
        return false;
    }

    public static bool IsCsv(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".csv")
        {
            return true;
        }
        return false;
    }

    public static bool IsXml(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".xml" || Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".json")
        {
            return true;
        }
        return false;
    }

    public static bool IsJson(string filename)
    {
        if (Path.GetExtension(filename).ToLower(CultureInfo.CurrentCulture) == ".json")
        {
            return true;
        }
        return false;
    }
}

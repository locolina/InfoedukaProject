namespace InfoedukaAPI.Helpers;

public static class BooleanConverter
{
    public static int BoolToBit(bool b)
    {
        if (b == true)
        {
            return 1;
        }
        return 0;
    }

    public static bool BitToBool(int b)
    {
        if (b == 1)
        {
            return true;
        }
        else if (b == 0)
        {
            return false;
        }
        else
        {
            throw new("Cannot convert to Bool");
        }
    }
}
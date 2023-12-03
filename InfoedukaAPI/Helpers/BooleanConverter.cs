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
}
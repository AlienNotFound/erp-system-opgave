namespace ErpSystemOpgave.Ui;
public static class Utility
{
    public static float EaseOutCube(float x) => 1 - MathF.Pow(1 - x, 3.0f);
    public static float EaseOutQuad(float x) => 1 - MathF.Pow(1 - x, 2.0f);

    public static (object, string) GetProp(object target, string property)
    {
        string[] props = property.Split('.');
        if (props.Length == 1)
            return (target, property);

        if (target.GetType().GetProperty(props[0])?.GetValue(target) is object newTarget)
        {
            return GetProp(newTarget, string.Join('.', props[1..]));
        }
        throw new Exception("Looks like you fucked up");
    }

    public static int Mod(this int self, int mod)
    {
        return (self % mod + mod) % mod;
    }
    public static void Rotate(this ref int self, int increase, int max)
    {
        self += increase;
        self = (self % max + max) % max;
    }
    public static string PadCenter(this string self, int width)
    {
        return self.PadLeft(width - ((width - self.Length) / 2)).PadRight(width);
    }
    public static string PadCenter(this string self, int width, char fill)
    {
        return self.PadLeft(width - ((width - self.Length) / 2), fill).PadRight(width, fill);
    }
}
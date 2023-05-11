namespace Super_Auto_Mobs.Scripts.Extensions
{
    public static class MathfExtensions
    {
        public static int RepeatInt(int value, int length)
        {
            return (value % length + length) % length;
        }
    }
}
using UnityEngine;

namespace Super_Auto_Mobs
{
    public static class ColorExtensions
    {
        public static Color SetA(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
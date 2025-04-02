namespace Utilities
{
    public class EarthParametrs
    {
        public const float VirtualEarthRadius = 2.1f;
        public const float RealEarthRadius = 6378f;

        public static float RealToVirtualDistance(float distance) => distance * VirtualEarthRadius / RealEarthRadius;
        
    }
}
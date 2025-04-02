namespace Utilities
{
    public class EarthParametrs
    {
        public static float VirtualEargRadius = 2.1f;
        public static float RealEarthRadius = 6378f;

        public static float RealToVirtualDistance(float distance) => distance * VirtualEargRadius / RealEarthRadius;
        
    }
}
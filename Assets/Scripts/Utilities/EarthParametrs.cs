namespace Utilities
{
    /// <summary> Предоставляет параметры Земли и методы преобразования расстояний. </summary>
    public class EarthParametrs
    {
        /// <summary> Виртуальный радиус Земли в игровых единицах. </summary>
        public const float VirtualEarthRadius = 2.1f;
        
        /// <summary> Реальный радиус Земли в километрах. </summary>
        public const float RealEarthRadius = 6378f;

        /// <summary> Преобразует реальное расстояние в виртуальное. </summary>
        /// <param name="distance"> Расстояние в реальных километрах. </param>
        /// <returns> Соответствующее расстояние в виртуальных единицах. </returns>
        /// <remarks>
        /// Использует пропорцию: VirtualEarthRadius / RealEarthRadius
        /// для масштабирования реальных расстояний к игровому миру.
        /// </remarks>
        public static float RealToVirtualDistance(float distance) => distance * VirtualEarthRadius / RealEarthRadius;
    }
}
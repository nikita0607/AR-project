namespace Marks.MarkGenerator
{
    /// <summary> Контейнер для массива меток. Используется для десериализации JSON данных. </summary>
    [System.Serializable]
    public class MarksSer
    {
        /// <summary> Массив объектов меток. </summary>
        public MarkSer[] marks;
    }
}
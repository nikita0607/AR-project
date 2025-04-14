namespace Marks
{
    /// <summary> Класс, представляющий метку с позицией в ECI-координатах. </summary>
    public class Mark : GCSPositionable
    {
        /// <summary> Название метки. </summary>
        public string Name { get; set; }

        /// <summary> Дополнительная информация о метке. </summary>
        public string Info { get; set; }
    }
}
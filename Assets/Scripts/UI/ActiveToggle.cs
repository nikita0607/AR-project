using UnityEngine;

namespace UI
{
    /// <summary> Компонент для переключения активности GameObject. </summary>
    public class ActiveToggle : MonoBehaviour
    {
        /// <summary> Переключает состояние активности текущего GameObject. </summary>
        /// <remarks>
        /// Если объект активен - деактивирует его, и наоборот.
        /// Работает через стандартный gameObject.SetActive().
        /// </remarks>
        public void ToggleActive()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
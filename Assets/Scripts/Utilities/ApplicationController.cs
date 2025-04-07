using UnityEngine;

namespace Utilities
{
    /// <summary> Контроллер управления жизненным циклом приложения. </summary>
    public class ApplicationController : MonoBehaviour
    {
        /// <summary> Завершает работу приложения. </summary>
        public void Quit()
        {
            Application.Quit();
        }
    }
}
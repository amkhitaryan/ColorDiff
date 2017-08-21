using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CP_ColorDiff
{
    /// <summary>
    /// Создает анимированный курсор из файла 
    /// </summary>
    public class AnimatedCurs
    {
        [DllImport("User32.dll")]
        private static extern IntPtr LoadCursorFromFile(String str);
        /// <summary>
        /// Создает курсор
        /// </summary>
        /// <param name="fname">Путь к файлу</param>
        /// <returns>Анимированный курсор</returns>
        public static Cursor Create(string fname)
        {
            // Создаем определяемый платформой тип для представления укзателя(или дескриптора)
            IntPtr hc = LoadCursorFromFile(fname);
            // Удалось ли создать курсор
            // Если да - возвращаем созданный курсор
            if (!IntPtr.Zero.Equals(hc))
            {
                
                return new Cursor(hc);
            }
            // Если нет - выбрасываем исключение
            else
            {
                throw new ApplicationException("Не удалось создать курсор из файла " + fname);
            }
        }
    }

}


using System;
using System.Collections.Generic;
using System.Drawing;


namespace CP_ColorDiff
{
    public class Forms
    {
        // Конструктор
        public Forms()
        {
            _forms = new List<Form>();
        }

        // Добавляет форму содержащую информацию о координатах, толщине, цвете и принадлежностях в список всех форм
        public void NewForm(Point L, float W, Color C, int S)
        {
            _forms.Add(new Form(L, W, C, S));
        }

        // Хранит все формы
        public readonly List<Form> _forms;

        // Удаляет любые данные о точке в пределах определенного порогового значения
        public void RemoveForm(Point L, float threshold)
        {
            for (int i = 0; i < _forms.Count; i++)
            {
                // Определяет находиться ли точка в пределах определенного расстояния от точки для удаления
                if ((!(Math.Abs(L.X - _forms[i].Location.X) < threshold)) ||
                    (!(Math.Abs(L.Y - _forms[i].Location.Y) < threshold))) continue;

                // Удаляет все данные по этому индексу
                _forms.RemoveAt(i);

                // Проходит через остальную часть данных и добаляет дополнительно 1, чтобы определить их в качестве отдельных форм
                for (int n = i; n < _forms.Count; n++)
                {
                    _forms[n].FormNumber += 1;
                }
                //Возврат на шаг назад, чтобы не потерять точку
                i -= 1;
            }
        }

        // Возвращает форму по нужному индексу
        public Form GetForm(int index)
        {
            return _forms[index];
        }

        // Возвращает число сохраненных форм
        public int NumberOfForms()
        {
            return _forms.Count;
        }
    }
}


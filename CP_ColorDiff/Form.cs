using System.Drawing;


namespace CP_ColorDiff
{
    public class Form
    {
        public Point Location;          // Координаты точки
        public float Width;             // Толщина линии
        public Color Colour;            // Цвет линии
        public int FormNumber;         // Частьи какой формы она принадлежит

        // Конструктор
        public Form(Point L, float W, Color C, int S)
        {
            Location = L;               // Хранит координаты
            Width = W;                  // Хранит толщину
            Colour = C;                 // Хранит цвет
            FormNumber = S;            // Хранит номер формы
        }
    }

}

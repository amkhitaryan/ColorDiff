using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CP_ColorDiff
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        #region Camera and sign globals

        /// <summary>
        /// Текущий жест
        /// </summary>
        private Sign _sign;
        /// <summary>
        /// Код текущего жеста
        /// </summary>
        private int _signCode;
        /// <summary>
        /// Код последнего жеста
        /// </summary>
        private int _lastSignCode;
        /// <summary>
        /// Включен ли захват с камеры
        /// </summary>
        private bool _isCapturing = false;
        /// <summary>
        /// Координата центра пальца
        /// </summary>
        private Point _fingerCenter;

        #endregion Camera and sign globals

        #region Painting and other globals

        /// <summary>
        /// Анимированный курсор
        /// </summary>
        private Cursor _aCurs = null;
        /// <summary>
        /// Счетчик форм для их отдельной прорисовки
        /// </summary>
        private int _formNum = 0;
        /// <summary>
        /// Режим стирания
        /// </summary>
        private bool _isEraseing = false;
        /// <summary>
        /// Режим рисования
        /// </summary>
        private bool _isPainting = false;
        /// <summary>
        /// Все нарисованные формы
        /// </summary>
        private Forms _drawingForms = new Forms();
        /// <summary>
        /// Текущий инструмент (кисть/ластик)
        /// </summary>
        private bool _brush = true;
        /// <summary>
        /// Текущий цвет кисти
        /// </summary>
        private Color _currentColour = Color.Black;
        /// <summary>
        /// Последняя координата, чтобы избежать повторов
        /// </summary>
        private Point _lastPosition = new Point(0, 0);
        /// <summary>
        /// Текущая толщина кисти
        /// </summary>
        private float _currentWidth = 5;
        /// <summary>
        /// Какие настройки HSB модели для пальцев используем
        /// </summary>
        private string _HSBSettings;

        #endregion Painting and other globals

        public Form1()
        {
            InitializeComponent();
            // Включение двойной буферизации для увеличения производительности
            this.DoubleBuffered = true;
            pbCamera.GetType()
               .GetMethod("SetStyle",
                   System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
               .Invoke(pnDraw,
                   new object[]
                   {
                        ControlStyles.ResizeRedraw |
                        ControlStyles.ContainerControl |
                        ControlStyles.OptimizedDoubleBuffer |
                        ControlStyles.SupportsTransparentBackColor |
                        ControlStyles.UserPaint |
                        ControlStyles.AllPaintingInWmPaint |
                        ControlStyles.DoubleBuffer,
                        true
                   });
            pnDraw.GetType()
                .GetMethod("SetStyle",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(pnDraw,
                    new object[]
                    {
                        ControlStyles.ResizeRedraw |
                        ControlStyles.ContainerControl |
                        ControlStyles.OptimizedDoubleBuffer |
                        ControlStyles.SupportsTransparentBackColor |
                        ControlStyles.UserPaint |
                        ControlStyles.AllPaintingInWmPaint |
                        ControlStyles.DoubleBuffer,
                        true
                    });

            comboBox1.SelectedItem = comboBox1.Items[0];

            var def = new DefHSB()
            {
                MinBrightness = 0.195,
                MinSaturation = 0.25,
                YellowMinHue = 65.0,
                YellowMaxHue = 105.0,
                YellowMinSaturation = 0.254,
                YellowMaxSaturation = 0.66,
                YellowMinBrightness = 0.23,
                YellowMaxBrightness = 0.82,
                OrangeMinHue = 0.0,
                OrangeMaxHue = 64.0,
                OrangeMinSaturation = 0.215,
                OrangeMaxSaturation = 0.588,
                OrangeMinBrightness = 0.39,
                OrangeMaxBrightness = 0.78,
                GreenMinHue = 105.0,
                GreenMaxHue = 180.0,
                GreenMinSaturation = 0.37,
                GreenMaxSaturation = 0.8,
                GreenMinBrightness = 0.196,
                GreenMaxBrightness = 0.94,
                PinkMinHue = 260.0,
                PinkMaxHue = 360.0,
                PinkMinSaturation = 0.32,
                PinkMaxSaturation = 0.66,
                PinkMinBrightness = 0.47,
                PinkMaxBrightness = 0.74,
                BlueMinHue = 200,
                BlueMaxHue = 240,
                BlueMinSaturation = 0.35,
                BlueMaxSaturation = 1,
                BlueMinBrightness = 0.27,
                BlueMaxBrightness = 0.84
            };
            var ser = new Serialization();
            ser.SerializeObject(def, Path.Combine(Application.StartupPath, "DefaultHSBparameters"));
            ser = null;
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            // Останавливаем захват с камеры
            this.camCapture1.Stop();
        }

        private void btUndo_Click(object sender, EventArgs e)
        {
            // Отменяем последнюю нарисованную форму из списка всех форм
            if (_drawingForms._forms.Count == 0) return;
            _drawingForms._forms.RemoveAt(_drawingForms._forms.Count - 1);
            pnDraw.Refresh();
        }

        private void btChangeColor_Click(object sender, EventArgs e)
        {
            // Показываем меню для выбора нового цвета
            var d = colorDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                // Меняем текущий цвет на выбранный
                _currentColour = colorDialog1.Color;
            }
        }

        private void btClearPanel_Click(object sender, EventArgs e)
        {
            // Очищаем полотно, очищаем список форм
            _drawingForms = new Forms();
            pnDraw.Refresh();
        }

        private void nmBrushWidth_ValueChanged(object sender, EventArgs e)
        {
            // Меняем толщину кисти и переводим в число с плавающей запятой
            _currentWidth = Convert.ToSingle(nmBrushWidth.Value);
        }

        private void btStopStart_Click(object sender, EventArgs e)
        {
            _isCapturing = !_isCapturing;
            if (_isCapturing)
            {
                _HSBSettings = comboBox1.SelectedItem == comboBox1.Items[0] ? "Default" : "Custom";
                btStopStart.Text = "Остановить";
                // Устанавливаем размер захватываемого изображения с камеры
                this.camCapture1.CaptureHeight = 500;
                this.camCapture1.CaptureWidth = 620;
                // Устанавливаем промежуток времени между кадрами
                this.camCapture1.TimeToCapture_milliseconds = 10;

                _sign = new Sign();
                _fingerCenter = new Point();
                _signCode = (int)SignID.NOTHING_HAS_DETECTED;
                _lastSignCode = (int)SignID.NOTHING_HAS_DETECTED;

                // Начинаем захват с камеры
                this.camCapture1.Start(0);
            }
            else
            {
                btStopStart.Text = "Начать захват";
                this.camCapture1.Stop();
            }
        }

        /// <summary>
        /// Осуществляет рисование на панели
        /// </summary>
        private void pnDraw_Paint(object sender, PaintEventArgs e)
        {
            // Включаем сглаживание
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            // Рисуем линии
            for (int i = 0; i < _drawingForms.NumberOfForms() - 1; i++)
            {
                var f1 = _drawingForms.GetForm(i);
                var f2 = _drawingForms.GetForm(i + 1);
                // Убедимся, что два прилегающих номера форм являются частью одной формы
                if (f1.FormNumber != f2.FormNumber) continue;
                // Создаем новую кисть 
                var p = new Pen(f1.Colour, f1.Width)
                {
                    StartCap = System.Drawing.Drawing2D.LineCap.Round,
                    EndCap = System.Drawing.Drawing2D.LineCap.Round
                };
                // Рисуем линию между двумя прилегающими точками
                e.Graphics.DrawLine(p, f1.Location, f2.Location);
                // Избавляемся от кисти, когда закончили
                p.Dispose();
            }
        }

        private void pnDraw_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Если инструмент - кисть
            if (_brush)
            {
                // Включаем режим рисования, увеличиваем к-во фигур и сбрасываем последнее местоположение указателя
                _isPainting = true;
                _formNum++;
                _lastPosition = new Point(0, 0);
            }
            // Если инструмент - ластик, включаем режим стирания
            else
            {
                _isEraseing = true;
            }
        }

        /// <summary>
        /// Изображение захвачено
        /// </summary>
        private void CamCapture1_ImageCaptured(object source, CameraCapture.CameraEventArgs e)
        {
            //var s = comboBox1.SelectedItem == comboBox1.Items[0] ? "Default" : "Custom";
            var capturedImage = new Recognition(e.CamImage, _HSBSettings);
            capturedImage.GetHSBSectors();
            // Создаем список пальцев
            var fingers = capturedImage.PixelsOfFingers();
            // Проверяем, что все пальцы распознаны на изображении
            if (fingers.Any(t => t == null || t.Count == 0)) return;
            // Определяем номер текущего жеста
            _signCode = _sign.CurrentSign(fingers);

            if (Cursor.Current != null) this.Cursor = new Cursor(Cursor.Current.Handle); 
            Point newFingerCenter = _sign.Center(_sign.FingerRectangles[(int) FingersID.THUMB]);
            Point currentCursor = Cursor.Position;
            Cursor.Position = new Point(currentCursor.X - (newFingerCenter.X - _fingerCenter.X),
                currentCursor.Y + (newFingerCenter.Y - _fingerCenter.Y));
            Cursor.Clip = new Rectangle(this.Location, this.Size);
            this._fingerCenter = newFingerCenter;
            const float width = 300; // 200 const
            const float height = 250; // 150 const
            var brush = new SolidBrush(Color.Black);
            // Получаем изображение с нарисованными границами вокруг пальцев
            var imageWithBorderLines = capturedImage.GetImageWithBorderLines();
            //capturedImage = null;
            // Рассчитываем коэффициент масштабирования
            var scale = Math.Min(width / imageWithBorderLines.Width, height / imageWithBorderLines.Height);
            // Временное изображение для дальнейшей обработки
            var tmp = new Bitmap((int) width, (int) height);
            // Создаем поверхность для рисования из временного изображения
            var graph = Graphics.FromImage(tmp);
            // Устанавливаем коэффициент масштабирования для высоты и ширины изображения
            var scaleWidth = (int) (imageWithBorderLines.Width * scale);
            var scaleHeight = (int) (imageWithBorderLines.Height * scale);
            // Масштабируем и рисуем изображение
            graph.FillRectangle(brush, new RectangleF(0, 0, width, height));
            graph.DrawImage(imageWithBorderLines,
                new Rectangle(((int) width - scaleWidth) / 2, ((int) height - scaleHeight) / 2, scaleWidth,
                    scaleHeight));
            // Полученное изображение показываем
            this.pbCamera.Image = tmp;
            // Выполняем определенные действия при определенных жестах
            if (_signCode == (int) SignID.THUMB_INDEX_FINGER && _signCode != _lastSignCode)
            {
                if (!_brush)
                    _brush = true;
                MouseMotion.MouseEvent(MouseMotion.MouseEventFlags.LeftDown);
                _lastSignCode = _signCode;
            }
            else if (_lastSignCode == (int) SignID.THUMB_INDEX_FINGER
                && _signCode == (int) SignID.NOTHING_HAS_DETECTED)
            {
                MouseMotion.MouseEvent(MouseMotion.MouseEventFlags.LeftUp);
                _lastSignCode = _signCode;
            }

            else if (_signCode == (int) SignID.THUMB_MIDDLE_FINGER && _signCode != _lastSignCode)
            {
                if (_brush)
                    _brush = false;
                MouseMotion.MouseEvent(MouseMotion.MouseEventFlags.LeftDown);
                _lastSignCode = _signCode;
            }
            else if (_lastSignCode == (int) SignID.THUMB_MIDDLE_FINGER
                && _signCode == (int) SignID.NOTHING_HAS_DETECTED)
            {
                MouseMotion.MouseEvent(MouseMotion.MouseEventFlags.LeftUp);
                _lastSignCode = _signCode;
            }

            else if (_signCode == (int) SignID.THUMB_RING_FINGER && _signCode != _lastSignCode)
            {
                _lastSignCode = _signCode;
            }
            else if (_signCode == (int)SignID.THUMB_PINKY)
            {
                if (_drawingForms._forms.Count == 0) return;
                _drawingForms._forms.RemoveAt(_drawingForms._forms.Count - 1);
                pnDraw.Refresh();
                _lastSignCode = _signCode;
            }
            else if (_lastSignCode == (int) SignID.THUMB_RING_FINGER
                && _signCode == (int) SignID.NOTHING_HAS_DETECTED)
            {
                var rnd = new Random();
                _currentColour = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                _lastSignCode = _signCode;
            }
        }
        
        private void pnDraw_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_isPainting)
            {
                // Закончили рисовать
                _isPainting = false;
            }
            if (_isEraseing)
            {
                // Закончили стирать
                _isEraseing = false;
            }
        }

        protected void pnDraw_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Режим рисования
            if (_isPainting)
            {
                // Проверяем, что не на том же месте, что и в последний раз, чтобы не делать лишних вычислений
                if (_lastPosition != e.Location)
                {
                    // Текущие координаты сохраняем как последние
                    _lastPosition = e.Location;
                    // Сохраняем координаты, толщину линии, цвет и к-во фигур
                    _drawingForms.NewForm(_lastPosition, _currentWidth, _currentColour, _formNum);
                }
            }
            if (_isEraseing)
            {
                // Стираем любые точки в пределах порогового значения от указателя мыши
                _drawingForms.RemoveForm(e.Location, 15);
            }
            // Обновляем панель, чтобы она перерисовалась
            pnDraw.Refresh();
        }
        
        /// <summary>
        /// Меняет иконку курсора на панели рисования
        /// </summary>
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            // Если новый указатель не создан - создаем его
            if (_aCurs == null)
            {
                try
                {
                    _aCurs =
                        AnimatedCurs.Create(Path.Combine(Application.StartupPath, "Tiny Mario Working in Background.ani"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            // Меняем курсор на новый
            this.Cursor = _aCurs;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            // Возвращаем обычный курсор вне панели рисования
            this.Cursor = null;
        }

        private void btFingerColorSettings_Click(object sender, EventArgs e)
        {
            btStopStart.Text = "Начать захват";
            this.camCapture1.Stop();
            var fS = new FormSettings();
            fS.ShowDialog();
        }
    }
}
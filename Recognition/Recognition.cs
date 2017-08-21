using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;

public class Recognition
{

    public string HSBValues;

    /// <summary>
    /// Коэффициенты HSB цветовой модели для пальцев по умолчанию
    /// </summary>
    private readonly DefHSB _defHSB; //= new Serialization().DeSerializeObject<DefHSB>(Path.Combine(Application.StartupPath, "DefaultHSBparameters"));
    
    /// <summary>
    /// Границы пальцев
    /// </summary>
    private readonly List<PointData>[] _fingersBorderLine = new List<PointData>[5];

    /// <summary>
    /// Контейнеры для подходящих точек
    /// </summary>
    private List<FingerPoint> _orange;
    private List<FingerPoint> _green;
    private List<FingerPoint> _pink;
    private List<FingerPoint> _yellow;
    private List<FingerPoint> _blue;
    /// <summary>
    /// Список пальцев
    /// </summary>
    private List<List<FingerPoint>> _fingers;

    /// <summary>
    /// Исходное изображение
    /// </summary>
    private readonly Bitmap _sourceImage;

    /// <summary>
    /// Копия исходного изображения
    /// </summary>
    private BitmapData _sourceImageCopy;

    /// <summary>
    /// Массив байтов, где будет храниться изображение для обработки
    /// </summary>
    private byte[] _byteImage;

    /// <summary>
    /// Наличие блокировки изображения
    /// </summary>
    private bool _imageLocked;

    /// <summary>
    /// Изображение, в котором будут храниться цветные сектора
    /// </summary>
    private Bitmap _colorSectors;

    /// <summary>
    /// Приватный конструктор, который используют публичные
    /// </summary>
    private void PrivRecognition()
    {
        _colorSectors = new Bitmap(_sourceImage.Width, _sourceImage.Height);

        _orange = new List<FingerPoint>();
        _green = new List<FingerPoint>();
        _blue = new List<FingerPoint>();
        _yellow = new List<FingerPoint>();
        _pink = new List<FingerPoint>();

        _fingers = new List<List<FingerPoint>> { _orange, _green, _blue, _yellow, _pink };

        for (int i = 0; i < _fingersBorderLine.Length; i++)
        {
            _fingersBorderLine[i] = new List<PointData>();
        }
        _imageLocked = false;
    }

    /// <summary>
    /// Блокировка изображения
    /// </summary>
    private void LockImage()
    {
        // Проверка на наличие блокировки
        if (_imageLocked) return;

        // Создание прямоугольника для дальнейшей блокировки
        var borderline = new Rectangle(0, 0,
                               _sourceImage.Width, _sourceImage.Height);

        // Блокировка данных исходного изображения, накладывая созданный прямоугольник
        _sourceImageCopy = _sourceImage.LockBits(borderline,
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb);

        // Определение длины массива для копирования
        int length = _sourceImageCopy.Stride * _sourceImageCopy.Height;
        _byteImage = new byte[length];

        // Копирование данных в массив байтов для дальнейшей обработки
        Marshal.Copy(_sourceImageCopy.Scan0, _byteImage, 0, length);

        // Изображение заблокировано
        _imageLocked = true;
    }

    /// <summary>
    /// Создает экземпляр класса
    /// </summary>
    /// <param name="path">Путь к исходному изображению</param>
    public Recognition(string path)
    {
        _sourceImage = LoadImage(path);

        PrivRecognition();
    }

    /// <summary>
    /// Определение информации argb изображения в конкретных координатах
    /// </summary>
    /// <param name="sourceImage">Исходное изображение</param>
    /// <param name="x">X координата</param>
    /// <param name="y">Y координата</param>
    public Color ARGBPixelColor(BitmapData sourceImage, int x, int y)
    {
        int i = y * sourceImage.Stride + x * 4;

        // Синий
        int blue = (int)_byteImage[i++];
        // Зеленый
        int green = (int)_byteImage[i++];
        // Красный
        int red = (int)_byteImage[i++];
        // Степень прозрачности
        int alpha = (int)_byteImage[i];

        Color pixel = Color.FromArgb(alpha, red, green, blue);

        return pixel;
    }

    /// <summary>
	/// Создает экземпляр класса
	/// </summary>
	/// <param name="image">Исходное изображение</param>
	public Recognition(Image image, string s)
    {
        _defHSB = new Serialization().DeSerializeObject<DefHSB>(Path.Combine(Application.StartupPath, s + "HSBparameters"));
        _sourceImage = new Bitmap(image);

        PrivRecognition();
    }

    /// <summary>
    /// Определение пикселей для каждого пальца
    /// </summary>
    /// <returns>Массив списков точек 5 пальцев</returns>
    public List<PointData>[] PixelsOfFingers()
    {
        return _fingersBorderLine;
    }

    /// <summary>
    /// Загрузка изображения из файла
    /// </summary>
    /// <returns>Загруженное изображение</returns>
    /// <param name="fileName">Имя загружаемого изображения</param>
    public static Bitmap LoadImage(string fileName)
    {
        using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            return new Bitmap(fs);
    }

    /// <summary>
    /// Кластеризация секторов
    /// </summary>
    private void SectorClustering()
    {
        // Новый список пальцев
        var newFingers = new List<PointData[]>();

        // Проход по секторам
        foreach (var oldFinger in _fingers)
        {
            // Для выделенных областей
            var clusters = new HashSet<PointData[]>();

            // Конвертация списка FingerPoint в список PointData
            var cluster = new List<PointData>();

            // Точки из исходного сектора добавляем в общий список для всех точек
            foreach (var item in oldFinger)
            {
                cluster.Add(new PointData(item.Point.X, item.Point.Y, item.Color, item.HSBColor));
            }

            // Создаем алгоритм подсчитывания расстояния (метрики)
            var dbs = new Clustering((x, y) => Math.Sqrt(((x.X - y.X) * (x.X - y.X)) + ((x.Y - y.Y) * (x.Y - y.Y))));
            // Сбор кластеров из исходного сектора
            dbs.ComputeCluster(allPoints: cluster.ToArray(), epsilon: 7.5, minPts: 8, clusters: out clusters);


            if (clusters.Count == 0) continue;
            {
                // Поиск самого большого кластера
                var maxPoints = clusters.Max(item => item.Length);
                // Сохранение самого большого кластера
                var currFinger = clusters.First(item => item.Length == maxPoints);
                newFingers.Add(currFinger);

                // Определение цвета пальца
                var fingerHue = currFinger.Average(item => item.HSBColor.Hue);
                
                if ((fingerHue >= _defHSB.YellowMinHue) && (fingerHue <= _defHSB.YellowMaxHue)) // Для желтого
                    _fingersBorderLine[(int)FingersColor.YELLOW] = currFinger.ToList();
                else if (fingerHue >= _defHSB.GreenMinHue && fingerHue <= _defHSB.GreenMaxHue) // Для зеленого
                    _fingersBorderLine[(int)FingersColor.GREEN] = currFinger.ToList();
                else if (fingerHue >= _defHSB.OrangeMinHue && fingerHue <= _defHSB.OrangeMaxHue) // Для оранжевого
                    _fingersBorderLine[(int)FingersColor.ORANGE] = currFinger.ToList();
                else if (fingerHue >= _defHSB.BlueMinHue && fingerHue <= _defHSB.BlueMaxHue) // Для синего
                    _fingersBorderLine[(int)FingersColor.BLUE] = currFinger.ToList();
                else if (fingerHue >= _defHSB.PinkMinHue && fingerHue <= _defHSB.PinkMaxHue) // Для розового
                    _fingersBorderLine[(int)FingersColor.PINK] = currFinger.ToList();
            }
        }

        // Прорисовка границ на исходном изображении
        using (var graphics = Graphics.FromImage(_sourceImage))
        {
            foreach (var item in newFingers)
            {
                // Блокировка изображения
                // Определение цвета по цвету первого пикселя из массива
                // Разблокировка изображения
                LockImage();
                Color pixel = ARGBPixelColor(_sourceImageCopy, (int)item[0].X, (int)item[0].Y);
                UnlockImage();

                // Определение цвета границы по цвету первого пикселя
                var pen = new Pen(Color.FromArgb(pixel.R, pixel.G, pixel.B), 4);

                if (item.Length != 0)
                {
                    // Прорисовка контура границы
                    graphics.DrawRectangle(pen,
                        (float)item.Min(a => a.X),
                        (float)item.Min(a => a.Y),
                        (float)item.Max(a => a.X) - (float)item.Min(a => a.X),
                        (float)item.Max(a => a.Y) - (float)item.Min(a => a.Y));
                }
            }
        }
    }

    /// <summary>
    /// Разблокировка изображения
    /// </summary>
    void UnlockImage()
    {
        // Проверка на  наличие блокировки
        if (!_imageLocked) return;

        // Определение длины массива для копирования
        int length = _sourceImageCopy.Stride * _sourceImageCopy.Height;
        // Копирование данных обратно в копию исходного изображения
        Marshal.Copy(_byteImage, 0, _sourceImageCopy.Scan0, length);
        // Разблокировка изображения
        _sourceImage.UnlockBits(_sourceImageCopy);
        // Освобождение ресурсов
        _byteImage = null;
        _sourceImageCopy = null;

        // Изображение разблокировано
        _imageLocked = false;
    }

    /// <summary>
    /// Прорисовка цветных секторов к исходному изображению
    /// </summary>
    /// <returns>Новое обработанное изображение</returns>
    public Bitmap GetImageWithBorderLines()
    {
        return _sourceImage;
    }

    /// <summary>
    /// Ищет цветные сектора в изображении
    /// </summary>
    public void GetHSBSectors()
    {
        // Блокировка изображения для ускорения работы
        LockImage();

        // Проход по изображние с помощью вложенного цикла
        for (var i = 0; i < _sourceImageCopy.Width; i += 5)
        {
            for (var j = 0; j < _sourceImageCopy.Height; j += 5)
            {
                Color pixel = ARGBPixelColor(_sourceImageCopy, i, j);
                HSB hsbpixel = ConvertColors.RGBtoHSB(pixel.R, pixel.G, pixel.B);

                //Отсев неподходящих точек
                if (!(hsbpixel.Brightness >= _defHSB.MinBrightness) || !(hsbpixel.Saturation > _defHSB.MinSaturation)) continue;

                // Условие для желтой точки
                if (hsbpixel.Saturation >= _defHSB.YellowMinSaturation && hsbpixel.Saturation <= _defHSB.YellowMaxSaturation && hsbpixel.Brightness >= _defHSB.YellowMinBrightness &&
                    hsbpixel.Brightness <= _defHSB.YellowMaxBrightness && ((hsbpixel.Hue >= _defHSB.YellowMinHue) && (hsbpixel.Hue <= _defHSB.YellowMaxHue)))
                {
                    _yellow.Add(new FingerPoint(i, j, pixel, hsbpixel));
                    _colorSectors.SetPixel(i, j, pixel);
                    continue;
                }

                // Условие для зеленой точки
                if (hsbpixel.Saturation >= _defHSB.GreenMinSaturation && hsbpixel.Saturation <= _defHSB.GreenMaxSaturation && 
                    hsbpixel.Brightness <= _defHSB.GreenMaxBrightness && hsbpixel.Hue >= _defHSB.GreenMinHue && hsbpixel.Hue <= _defHSB.GreenMaxHue)
                {
                    _green.Add(new FingerPoint(i, j, pixel, hsbpixel));
                    _colorSectors.SetPixel(i, j, pixel);
                    continue;
                }

                // Условие для оранжевой точки
                if (hsbpixel.Saturation >= _defHSB.OrangeMinSaturation && hsbpixel.Saturation <= _defHSB.OrangeMaxSaturation && hsbpixel.Brightness >= _defHSB.OrangeMinBrightness &&
                    hsbpixel.Brightness <= _defHSB.OrangeMaxBrightness && (hsbpixel.Hue >= _defHSB.OrangeMinHue && hsbpixel.Hue <= _defHSB.OrangeMaxHue))
                {
                    _orange.Add(new FingerPoint(i, j, pixel, hsbpixel));
                    _colorSectors.SetPixel(i, j, pixel);
                    continue;
                }

                // Условие для синей точки
                if (hsbpixel.Saturation >= _defHSB.BlueMinSaturation && hsbpixel.Brightness >= _defHSB.BlueMinBrightness &&
                    hsbpixel.Brightness <= _defHSB.BlueMaxBrightness && hsbpixel.Hue >= _defHSB.BlueMinHue && hsbpixel.Hue <= _defHSB.BlueMaxHue)
                {
                    _blue.Add(new FingerPoint(i, j, pixel, hsbpixel));
                    _colorSectors.SetPixel(i, j, pixel);
                    continue;
                }

                // Условие для розовой точки
                if (hsbpixel.Saturation >= _defHSB.PinkMinSaturation && hsbpixel.Saturation <= _defHSB.PinkMaxSaturation && hsbpixel.Brightness >= _defHSB.PinkMinBrightness &&
                    hsbpixel.Brightness <= _defHSB.PinkMaxBrightness && hsbpixel.Hue >= _defHSB.PinkMinHue && hsbpixel.Hue <= _defHSB.PinkMaxHue)
                {
                    _pink.Add(new FingerPoint(i, j, pixel, hsbpixel));
                    _colorSectors.SetPixel(i, j, pixel);
                }
            }
        }
        // Разблокировка изображения и начало кластерного анализа
        UnlockImage();
       SectorClustering();
    }

    /// <summary>
    /// Прорисовка цветных границ и сохранение в указанный адрес
    /// Рисуем цветовые сектора и сохраняем по указанному адресу.
    /// </summary>
    /// <param name="fileName">Имя сохранённого файла.</param>
    public void PaintBorderLine(string fileName)
    {
        // Прорисовка границ на дополнительном изображении
        using (var graphics = Graphics.FromImage(_colorSectors))
        {
            var pen = new Pen(Color.Black, 1);

            foreach (var item in _fingers)
            {
                if (item.Count != 0)
                {
                    graphics.DrawRectangle(pen,
                        item.Min(a => a.Point.X),
                        item.Min(a => a.Point.Y),
                        item.Max(a => a.Point.X) - item.Min(a => a.Point.X),
                        item.Max(a => a.Point.Y) - item.Min(a => a.Point.Y));
                }
            }
            _colorSectors.Save(fileName + "colors_painted.png");
        }

        // Прорисовка границ на исходном изображении
        using (var graphics = Graphics.FromImage(_sourceImage))
        {
            var pen = new Pen(Color.Black, 1); 

            foreach (var item in _fingers)
            {
                if (item.Count != 0)
                {
                    graphics.DrawRectangle(pen,
                        item.Min(a => a.Point.X),
                        item.Min(a => a.Point.Y),
                        item.Max(a => a.Point.X) - item.Min(a => a.Point.X),
                        item.Max(a => a.Point.Y) - item.Min(a => a.Point.Y));
                }
            }
            _sourceImage.Save(fileName + "original_colors_painted.jpg");
        }
    }
}


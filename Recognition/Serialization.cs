using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Сериализует и десериализует объекты
/// </summary>
public class Serialization
{
    /// <summary>
    /// Сериализирует объект
    /// </summary>
    /// <param name="serializableObject">Объект, который нужно сериализовать</param>
    /// <param name="fileName">Путь и имя файла</param>
    public void SerializeObject<T>(T serializableObject, string fileName)
    {
        if (serializableObject == null) { return; }

        try
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(fileName);
                stream.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    /// <summary>
    /// Десериализует xml файл в список объектов
    /// </summary>
    /// <returns>Десериализованный объект</returns>
    public T DeSerializeObject<T>(string fileName)
    {
        if (string.IsNullOrEmpty(fileName)) { return default(T); }

        T objectOut = default(T);

        try
        {
            string attributeXml = string.Empty;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(T);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    objectOut = (T)serializer.Deserialize(reader);
                    reader.Close();
                }
                read.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return objectOut;
    }
}


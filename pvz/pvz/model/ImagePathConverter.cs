using System;
using System.Globalization;
using System.Windows.Data;
using System.IO;
namespace mvvmsample.model
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int prId)
            {                
                string projectPath = AppDomain.CurrentDomain.BaseDirectory;
                string photo = prId + ".jpg";
                string imagePath = Path.Combine(projectPath, "..", "..", "..", "..", "..", "..", "images", photo);
                string tempFolderPath = Path.GetTempPath();
                string tempImagePath = Path.Combine(tempFolderPath, photo);
                if (File.Exists(tempImagePath))
                {
                    return tempImagePath;
                }
                else
                {
                    File.Copy(imagePath, tempImagePath);
                    return tempImagePath;
                }
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
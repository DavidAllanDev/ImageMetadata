using System.Drawing;
using System.Drawing.Imaging;

namespace ImageMetadata
{
    public class ReadImageMetadata
    {
        public Image Metadata(string imagePath, bool filterChars = true)
        {
            if (imagePath == null)
                throw new ApplicationException("The image path is not valid!");

            if (!File.Exists(imagePath))
                throw new ApplicationException($"The image for this path:[{imagePath}] does not exists!");

            Image image = new() { Name = GetImageName(imagePath), Description = GetImageDescription(imagePath), Path = imagePath, MetadataList = [] };

            Bitmap bitmap = new(filename: imagePath);

            PropertyItem[] propItems = bitmap.PropertyItems;

            int count = 0;

            System.Text.ASCIIEncoding encoding = new();

            foreach (PropertyItem propItem in propItems)
            {
                string imageMetadataValue = string.Empty;

                if (propItem != null && propItems[count] != null && propItems[count].Value != null)
                {
                    byte[]? propValue = propItems[count].Value;

                    if (propValue != null)
                    {
                        if (filterChars)
                            imageMetadataValue = ReplaceJavaChars(encoding.GetString(propValue));
                        else
                            imageMetadataValue = encoding.GetString(propValue);
                    }
                }

                count++;

                string ImageMetadataId = string.Empty;

                if (propItem != null && propItem.Id > 0)
                {
                    //https://exiftool.org/TagNames/EXIF.html (https://exiftool.org/)
                    ImageMetadataId = $"0x{propItem.Id:x}";// The "0x8773" seems to have platform info such as (Google) so Android
                }

                ImageMetadata meta = new() { Id = ImageMetadataId, Type = propItem.Type.ToString(), Value = imageMetadataValue };

                if (filterChars)
                {
                    if (string.IsNullOrEmpty(meta.Value.Trim()))
                        continue;

                    if (!meta.Type.Equals("1") && !meta.Type.Equals("2") && !meta.Type.Equals("3") && !meta.Type.Equals("5"))
                        continue;

                    if (image.MetadataList.Any(M => M.Value == meta.Value && M.Type == meta.Type))
                        continue;
                }


                image.MetadataList.Add(meta);
            }

            return image;
        }

        /// <summary>
        /// \b             /* \u0008: backspace (BS) */
        /// \t             /* \u0009: horizontal tab (HT) */
        /// \n             /* \u000a: linefeed (LF) */
        /// \f             /* \u000c: form feed (FF) */
        /// \r             /* \u000d: carriage return (CR) */
        /// \"             /* \u0022: double quote (") */
        /// \'             /* \u0027: single quote (') */
        /// \\             /* \u005c: backslash (\) */
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static string ReplaceJavaChars(string v) => v.Replace("\0", "")
                                                             .Replace("\u0008", "")
                                                             .Replace("\u0009", "")
                                                             .Replace("\u000a", "\n")
                                                             .Replace("u000c", "\n")
                                                             .Replace("\u000d", "\r")
                                                             .Replace("\u0022", "\"")
                                                             .Replace("\u0027", "'")
                                                             .Replace("\u005c", "\\")
                                                             .Replace("\u0002", "")
                                                             .Replace("\u0001", "")
                                                             .Replace("\u0006", "");

        private string GetImageDescription(string imagePath)
        {
            FileInfo fileInfo = new(imagePath);

            var ext = fileInfo.Extension;

            string result = fileInfo.FullName.Replace(fileInfo.DirectoryName, "").Replace("\\", "").Replace(ext, "");

            return result;
        }

        private string GetImageName(string imagePath)
        {
            FileInfo fileInfo = new(imagePath);

            string result = fileInfo.FullName.Replace(fileInfo.DirectoryName, "").Replace("\\", "");

            return result;
        }
    }
}

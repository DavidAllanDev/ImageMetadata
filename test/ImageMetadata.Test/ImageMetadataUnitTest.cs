namespace ImageMetadata.Test
{
    [TestClass]
    public class ImageMetadataUnitTest
    {
        [TestMethod]
        public void ImageMetadataPathTest()
        {
            //TDOD: Before test place you file path below:
            string finalPath = @"!!PLACE IMAGE PATH HERE!!"; ;

            if (!File.Exists(finalPath))
                Assert.Fail("No able to Test it check the file for test in ImageMetadata\\test\\TesFiles path.");

            ReadImageMetadata imageMetadata = new();

            Image img = imageMetadata.Metadata(finalPath, filterChars: true);

            Assert.IsNotNull(img);
            Assert.IsNotNull(img.MetadataList);
            Assert.IsTrue(img.MetadataList.Count > 0);
        }        
        
        [TestMethod]
        public void ImageMetadataTest()
        {
            string imageFileName = "20210624_150302.jpg";

            string imagePath = GetImagePathForTest();

            string finalPath = Path.Combine(imagePath, imageFileName);

            if (!File.Exists(finalPath))
                Assert.Fail("No able to Test it check the file for test in ImageMetadata\\test\\TesFiles path.");

            ReadImageMetadata imageMetadata = new();

            Image img = imageMetadata.Metadata(finalPath);

            Assert.IsNotNull(img);
            Assert.IsNotNull(img.MetadataList);
            Assert.IsTrue(img.MetadataList.Count > 0);
        }

        public static string GetImagePathForTest()
        {
            string sourcePath = @"TesFiles\";
            int tries = 0;

            do
            {
                tries++;
                sourcePath = "..\\" + sourcePath;

                if (tries > 10)
                    return string.Empty; ;

            } while (!Directory.Exists(sourcePath));

            return sourcePath;
        }
    }
}
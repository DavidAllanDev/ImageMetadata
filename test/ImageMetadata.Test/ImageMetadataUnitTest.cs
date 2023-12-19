namespace ImageMetadata.Test
{
    [TestClass]
    public class ImageMetadataUnitTest
    {
        [TestMethod]
        public void ImageMetadataTest()
        {
            string imagePath = @"C:\dev\S\DO\20210624_150302.jpg";

            ReadImageMetadata imageMetadata = new();

            Image img = imageMetadata.Metadata(imagePath);

            Assert.IsNotNull(img);
            Assert.IsNotNull(img.MetadataList);
            Assert.IsTrue(img.MetadataList.Count > 0);
        }
    }
}
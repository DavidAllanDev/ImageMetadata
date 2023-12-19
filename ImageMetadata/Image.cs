namespace ImageMetadata
{
    public class Image
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }

        public List<ImageMetadata> MetadataList { get; set; }
    }
}

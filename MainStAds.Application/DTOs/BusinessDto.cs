namespace MainStAds.Application.DTOs
{
    public class BusinessDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Link { get; set; }
        public string ImageType { get; set; }
        public string ImageDataAsBase64 => Convert.ToBase64String(ImageData);
        public byte[] ImageData { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}

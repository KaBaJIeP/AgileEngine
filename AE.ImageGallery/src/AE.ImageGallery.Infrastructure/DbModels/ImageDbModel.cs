namespace AE.ImageGallery.Infrastructure.DbModels
{
    public class ImageDbModel
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Camera { get; set; }
        public string Tags { get; set; }
        public string CroppedPicture { get; set; }
        public string FullPicture { get; set; }
    }
}
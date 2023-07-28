namespace API.Services
{
    public interface IUploadService
    {
        public Task UploadImage(IFormFile _image, int _id);
    }
}

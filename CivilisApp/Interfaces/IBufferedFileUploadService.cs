using Microsoft.AspNetCore.WebUtilities;
namespace CivilisApp.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file);
    }
}

using CivilisApp.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CivilisApp.ApiTests;

internal sealed class TestBufferedFileUploadService : IBufferedFileUploadService
{
    public Task<bool> UploadFile(IFormFile file) => Task.FromResult(file.Length > 0);
}

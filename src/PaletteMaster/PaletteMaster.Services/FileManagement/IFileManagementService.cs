using PaletteMaster.Models;
using PaletteMaster.Models.DTO.FileManagement;

namespace PaletteMaster.Services.FileManagement;

public interface IFileManagementService
{
    public Task<Result<LoadFolderResponse, HandledException>> LoadFolderAsync(LoadFolderRequest request);

    public Task<Result<SaveFilesToFolderResponse, HandledException>> SaveFilesToFolderAsync(
        SaveFilesToFolderRequest request);
}
using PaletteMaster.Models;
using PaletteMaster.Models.DTO.FileManagement;
using PaletteMaster.Services.FileManagement;
using PaletteMaster.Services.Tests.Utilities;

namespace PaletteMaster.Services.Tests;

public class FileManagementUnitTests
{
    private readonly IFileManagementService _fileManagementService;

    private const string ThreeItemsFolderName = "ThreeItems";
    private const string OutputFolderName = "Output";
    private const string SubFolderName = "SubFolder";
    private const string SubSubfolderName = "Sub";
    
    private const string Image1Name = "cartoonGuy.png";
    private const string Image2Name = "orc07.png";
    private const string Image3Name = "Pixel_art_Wizard_Portrait.png";
    
    public FileManagementUnitTests()
    {
        _fileManagementService = new FileManagementService();
    }

    [Fact]
    public async Task LoadFolderAsync_ThreeImages_ReturnsThreeFileResponses()
    {
        // Arrange
        string fullPath = TestUtility.GetSamplePath(ThreeItemsFolderName);
        LoadFolderRequest request = new(fullPath);
        
        
        string image1Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image1Name);
       
        string image2Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image2Name);
        
        string image3Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image3Name);
        
        int expectedFiles = 3;
        
        // Act
        Result<LoadFolderResponse, HandledException> result = await _fileManagementService.LoadFolderAsync(request);

        LoadFolderResponse? response = result.Match<LoadFolderResponse?>(
            success: response => response,
            failure: error => null
            );

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedFiles, response.Files.Count);
        
        Assert.Contains(response.Files, fileResponse => fileResponse.FileName == Image1Name);
        Assert.Contains(response.Files, fileResponse => fileResponse.Path == image1Path);
        
        Assert.Contains(response.Files, fileResponse => fileResponse.FileName == Image2Name);
        Assert.Contains(response.Files, fileResponse => fileResponse.Path == image2Path);
        
        Assert.Contains(response.Files, fileResponse => fileResponse.FileName == Image3Name);
        Assert.Contains(response.Files, fileResponse => fileResponse.Path == image3Path);
    }

    [Fact]
    public async Task LoadFolderAsync_SubFolders_ReturnsFilesCorrectRelativePaths()
    {
        // Arrange
        string fullPath = TestUtility.GetSamplePath(SubFolderName);
        LoadFolderRequest request = new(fullPath);
        
        //string image1Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image1Name);
       
        string image2Path = Path.Combine(TestUtility.GetSamplePath(SubFolderName), Image2Name);
        
        string image3Path = Path.Combine(TestUtility.GetSamplePath(SubFolderName), SubSubfolderName, Image3Name);
        
        int expectedFiles = 2;
        
        // Act
        Result<LoadFolderResponse, HandledException> result = await _fileManagementService.LoadFolderAsync(request);

        LoadFolderResponse? response = result.Match<LoadFolderResponse?>(
            success: response => response,
            failure: error => null
        );

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedFiles, response.Files.Count);
        
        Assert.Contains(response.Files, fileResponse => fileResponse.FileName == Image2Name);
        Assert.Contains(response.Files, fileResponse => fileResponse.Path == image2Path);
        
        Assert.Contains(response.Files, fileResponse => fileResponse.FileName == Image3Name);
        Assert.Contains(response.Files, fileResponse => fileResponse.Path == image3Path);
    }

    [Fact]
    public async Task LoadFolderAsync_PathIsEmptyString_ReturnsHandledException()
    {
        // Arrange
        LoadFolderRequest request = new("");
        
        // Act
        Result<LoadFolderResponse, HandledException> result = await _fileManagementService.LoadFolderAsync(request);

        HandledException? exception = result.Match<HandledException?>(
            success: response => null,
            failure: error => error
            );
        
        // Assert
        Assert.NotNull(exception);
    }

    [Fact]
    public async Task SaveFilesToFolderAsync_ThreeFiles_SavesThreeItemsToOutputFolder()
    {
        // Arrange
        string outputPath = TestUtility.GetSamplePath(OutputFolderName);
        
        // Delete all the files in the output folder, but leave the base output folder existing
        if (Directory.Exists(outputPath))
        {
            DirectoryInfo outputDirectory = new(outputPath);
            foreach (FileInfo file in outputDirectory.GetFiles())
            {
                file.Delete();
            }
        }
        else
        {
            // Create directory
            Directory.CreateDirectory(outputPath);
        }

        List<FileToSave> filesToSave = new();
        
        string image1Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image1Name);
        FileStream image1Stream = File.OpenRead(image1Path);
        MemoryStream image1MemoryStream = new();
        await image1Stream.CopyToAsync(image1MemoryStream);
        FileToSave image1FileToSave = new FileToSave(Image1Name, Image1Name, image1MemoryStream);
        filesToSave.Add(image1FileToSave);
        
        string image2Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image2Name);
        FileStream image2Stream = File.OpenRead(image2Path);
        MemoryStream image2MemoryStream = new();
        await image2Stream.CopyToAsync(image2MemoryStream);
        FileToSave image2FileToSave = new FileToSave(Image2Name, Image2Name, image2MemoryStream);
        
        filesToSave.Add(image2FileToSave);
        
        string image3Path = Path.Combine(TestUtility.GetSamplePath(ThreeItemsFolderName), Image3Name);
        FileStream image3Stream = File.OpenRead(image3Path);
        MemoryStream image3MemoryStream = new();
        await image3Stream.CopyToAsync(image3MemoryStream);
        FileToSave image3FileToSave = new FileToSave(Image3Name, Image3Name, image3MemoryStream);
        filesToSave.Add(image3FileToSave);
        
        SaveFilesToFolderRequest request = new(filesToSave, outputPath);
        
        // Act
        Result<SaveFilesToFolderResponse, HandledException> result = await _fileManagementService.SaveFilesToFolderAsync(request);

        var (response, error) = result.Match<(SaveFilesToFolderResponse?, HandledException?)>(
            success: response => (response, null),
            failure: error => (null, error)
            );
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.FilesToSave.Count, response.TotalFilesSaved);
        
        Assert.True(File.Exists(Path.Combine(outputPath, Image1Name)));
        Assert.True(File.Exists(Path.Combine(outputPath, Image2Name)));
        Assert.True(File.Exists(Path.Combine(outputPath, Image3Name)));
    }
    
    [Fact]
    public async Task SaveFilesToFolderAsync_TwoFilesSubfolder_SavesTwoItemsToOutputFolderIncludingSubfolder()
    {
        // Arrange
        string outputPath = TestUtility.GetSamplePath(OutputFolderName);
        
        // Delete all the files in the output folder, but leave the base output folder existing
        if (Directory.Exists(outputPath))
        {
            DirectoryInfo outputDirectory = new(outputPath);
            foreach (FileInfo file in outputDirectory.GetFiles())
            {
                file.Delete();
            }
        }
        else
        {
            // Create directory
            Directory.CreateDirectory(outputPath);
        }

        List<FileToSave> filesToSave = new();
        
        string image2Path = Path.Combine(TestUtility.GetSamplePath(SubFolderName), Image2Name);
        FileStream image2Stream = File.OpenRead(image2Path);
        MemoryStream image2MemoryStream = new();
        await image2Stream.CopyToAsync(image2MemoryStream);
        FileToSave image2FileToSave = new FileToSave(Image2Name, Image2Name, image2MemoryStream);
        
        filesToSave.Add(image2FileToSave);
        
        string image3Path = Path.Combine(TestUtility.GetSamplePath(SubFolderName), SubSubfolderName, Image3Name);
        FileStream image3Stream = File.OpenRead(image3Path);
        MemoryStream image3MemoryStream = new();
        await image3Stream.CopyToAsync(image3MemoryStream);
        FileToSave image3FileToSave = new FileToSave(Image3Name, Path.Combine(SubSubfolderName, Image3Name), image3MemoryStream);
        filesToSave.Add(image3FileToSave);
        
        SaveFilesToFolderRequest request = new(filesToSave, outputPath);
        
        // Act
        Result<SaveFilesToFolderResponse, HandledException> result = await _fileManagementService.SaveFilesToFolderAsync(request);

        var (response, error) = result.Match<(SaveFilesToFolderResponse?, HandledException?)>(
            success: response => (response, null),
            failure: error => (null, error)
            );
        
        // Assert
        Assert.NotNull(response);
        Assert.Equal(request.FilesToSave.Count, response.TotalFilesSaved);
        
        Assert.True(File.Exists(Path.Combine(outputPath, Image2Name)));
        Assert.True(File.Exists(Path.Combine(outputPath, SubSubfolderName, Image3Name)));
    }
}
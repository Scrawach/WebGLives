using CSharpFunctionalExtensions;
using WebGLives.Core;
using WebGLives.Core.Errors;

namespace WebGLives.BusinessLogic.Services.Abstract;

public interface IFilesService
{
    Task<Result<string, Error>> Save(string folderName, FileData file, CancellationToken token = default);
    Task<Result<string, Error>> SaveZip(string folderName, FileData gameArchive, CancellationToken token = default);
    UnitResult<Error> Delete(string folderName);
}
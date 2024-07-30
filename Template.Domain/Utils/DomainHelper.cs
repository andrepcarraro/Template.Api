using Microsoft.AspNetCore.Http;

namespace Template.Domain;
public class DomainHelper
{
    public static bool IsExcelFile(IFormFile file)
    {
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        return fileExtension == ".xls" || fileExtension == ".xlsx";
    }
}

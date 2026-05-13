namespace HRManagement.MVC.Models.Common;

public class ErrorVM
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

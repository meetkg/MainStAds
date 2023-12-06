namespace MainStAds.Application.DTOs
{
    public class ErrorViewModelDto
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

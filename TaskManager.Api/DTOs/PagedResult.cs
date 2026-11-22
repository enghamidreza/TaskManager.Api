namespace TaskManager.Api.DTOs;

public class PagedResult<T>
{
    // جنریک تایپ هستش ک بعدا نوع لیست خروجی ک از چ کلاسی باشه مشخص میشه:List<T> 
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    //Ceiling :روند ب بالا میکنه 
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
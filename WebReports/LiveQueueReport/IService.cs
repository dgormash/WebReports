namespace WebReports.LiveQueueReport
{
    public interface IService
    {
        string Id { get; set; }
        string Name { get; set; } 
        string State { get; set; }
        string Code { get; set; }
    }
}
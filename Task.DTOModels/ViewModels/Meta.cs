namespace Task.DTOModels.ViewModels
{
    public class Meta
    {
        public Meta(int total, int pageSize, int pageNumber)
        {
            Total = total;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public int Total { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}

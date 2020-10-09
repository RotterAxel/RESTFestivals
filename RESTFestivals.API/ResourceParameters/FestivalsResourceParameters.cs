namespace RESTFestivals.API.ResourceParameters
{
    public class FestivalsResourceParameters
    {
        const int MaxPageSize = 48;

        public string SearchQuery { get; set; }

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public bool TodayOrLater { get; set; } = true;

        public string OrderBy { get; set; } = "StartDate";
    }
}

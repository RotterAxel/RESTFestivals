using System.Collections.Generic;

namespace RESTFestivals.CSVConverter.Entity
{
    public class Festival
    {
        public string FestivalUrl { get; set; }
        public string Title { get; set; }
        public FestivalStatus? Status { get; set; }
        public int? AverageExpectedVisitors { get; set; }
        public bool? IsCampingPossible { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public ICollection<Artists> Artists { get; set; }
            = new List<Artists>();
        public ICollection<FestivalDate> Dates { get; set; }
            = new List<FestivalDate>();

        public Address Address { get; set; }
    }
}
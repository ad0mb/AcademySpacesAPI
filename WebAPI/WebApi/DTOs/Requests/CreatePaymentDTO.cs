namespace AcademySpacesAPI.WebApi.DTOs.Requests
{
    public class CreatePaymentDTO
    {
        public int schoolId { get; set; }
        public int StudentId { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }
        public int ProssesedBy { get; set; }
        public DateTime date { get; set; }
        public string recipt_number { get; set; }
        public string fee_type { get; set; }
    }
}
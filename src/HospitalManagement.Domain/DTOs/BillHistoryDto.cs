using System;

namespace HospitalManagement.Domain.DTOs
{
    public class BillHistoryDto
    {
        public string Date { get; set; }
        public string DoctorName { get; set; }
        public decimal BillAmount { get; set; }
        public string BillStatus { get; set; }
    }
}

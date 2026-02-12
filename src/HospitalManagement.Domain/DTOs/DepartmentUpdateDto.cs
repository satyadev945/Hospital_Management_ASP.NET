using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.DTOs
{
    public class DepartmentUpdateDto
    {
        [Required]
        public int DeptNo { get; set; }

        [StringLength(30)]
        public string DeptName { get; set; }

        /// <summary>
        /// Alias for DeptName (for backward compatibility)
        /// </summary>
        public string DepartmentName
        {
            get => DeptName;
            set => DeptName = value;
        }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}

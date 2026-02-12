using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services
{
    public interface IDepartmentService
    {
        /// <summary>
        /// Gets a department by ID
        /// </summary>
        Task<DepartmentDto> GetByIdAsync(int deptNo);

        /// <summary>
        /// Gets all departments
        /// </summary>
        Task<IEnumerable<DepartmentDto>> GetAllAsync();

        /// <summary>
        /// Gets department by name
        /// </summary>
        Task<DepartmentDto> GetByNameAsync(string departmentName);

        /// <summary>
        /// Creates a new department
        /// </summary>
        Task<DepartmentDto> CreateAsync(DepartmentCreateDto departmentCreateDto);

        /// <summary>
        /// Updates an existing department
        /// </summary>
        Task<DepartmentDto> UpdateAsync(DepartmentUpdateDto departmentUpdateDto);

        /// <summary>
        /// Deletes a department by ID
        /// </summary>
        Task<bool> DeleteAsync(int deptNo);

        /// <summary>
        /// Gets department information with doctor count
        /// </summary>
        Task<IEnumerable<DepartmentDto>> GetDepartmentInfoAsync();

        /// <summary>
        /// Gets doctors in a specific department
        /// </summary>
        Task<IEnumerable<DoctorDto>> GetDepartmentDoctorsAsync(string departmentName);

        /// <summary>
        /// Checks if department exists by name
        /// </summary>
        Task<bool> ExistsByNameAsync(string departmentName);
    }
}

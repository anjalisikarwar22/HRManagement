namespace HRManagement.API.DTOs.Departments
{
    // ========================= MAIN DTO =========================

    public class DepartmentDto
    {
        public decimal DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public decimal? ManagerId { get; set; }

        public decimal? LocationId { get; set; }
    }

    // ========================= LIST DTO =========================

    public class DepartmentListDto
    {
        public decimal DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public decimal? ManagerId { get; set; }

        public decimal? LocationId { get; set; }
    }

    // ========================= CREATE DTO =========================

    public class CreateDepartmentDto
    {
        public string DepartmentName { get; set; } = string.Empty;

        public decimal? ManagerId { get; set; }

        public decimal? LocationId { get; set; }
    }

    // ========================= UPDATE DTO =========================

    public class UpdateDepartmentDto
    {
        public string DepartmentName { get; set; } = string.Empty;

        public decimal? ManagerId { get; set; }

        public decimal? LocationId { get; set; }
    }

    // ========================= DROPDOWN DTO =========================

    public class DepartmentDropdownDto
    {
        public decimal DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;
    }

    // ========================= PAGED DTO =========================

    public class PagedDepartmentDto
    {
        public IEnumerable<DepartmentListDto> Items { get; set; }
            = new List<DepartmentListDto>();

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }

    // ========================= SUMMARY DTO =========================

    public class DepartmentSummaryDto
    {
        public int TotalDepartments { get; set; }

        public int DepartmentsWithManager { get; set; }

        public int DepartmentsWithoutManager { get; set; }

        public int DepartmentsWithLocation { get; set; }

        public int DepartmentsWithoutLocation { get; set; }

        public double AverageEmployeesPerDepartment { get; set; }
    }
}
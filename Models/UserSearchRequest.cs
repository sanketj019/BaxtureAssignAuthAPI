namespace BaxtureAssignAuthAPI.Models
{
    public class UserSearchRequest
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
    }

}

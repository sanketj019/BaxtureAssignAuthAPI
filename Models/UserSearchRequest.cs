namespace BaxtureAssignAuthAPI.Models
{
    public class UserSearchRequest
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string SortBy { get; set; }
        public bool SortDescending { get; set; }
    }

}

namespace BaxtureAssignAuthAPI.Repository.IRepository
{
    public interface IJWTServiceRepository
    {
        public string GenerateToken(string userId, string userName, bool isAdmin);
    }
}

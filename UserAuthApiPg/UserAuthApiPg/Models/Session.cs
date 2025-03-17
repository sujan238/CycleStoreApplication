namespace UserAuthApiPg.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SessionToken { get; set; } // Unique token for the session
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; } // Optional expiration
        public bool IsActive { get; set; }

        public User User { get; set; } // Navigation property
    }
}
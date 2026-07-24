public record RegisterRequest(string FullName, string Email, string Password, UserRole Role);
public record LoginRequest(string Email, string Password);
public record AuthResponse(string Token, string FullName, string Role);
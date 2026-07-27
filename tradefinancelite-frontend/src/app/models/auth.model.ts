export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  fullName: string;
  role: string;
}

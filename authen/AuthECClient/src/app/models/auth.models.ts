export interface RegisterRequest {
  username: string;
  password: string;
  fullName: string;
  email: string;
  db: {
    Username: string;
    fullName: string;
    email: string;
  };
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  message?: string;
}

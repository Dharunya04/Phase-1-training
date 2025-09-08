
export interface RegisterRequest {
  username: string;
  password: string;
  email: string;
  phoneNumber: string;
  roleName: string;
}
export interface RegisterResponse {
  message: string;
}
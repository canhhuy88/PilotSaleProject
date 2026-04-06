export interface User {
  id: string;
  username: string;
  fullName: string;
  role: 'Admin' | 'Staff';
  token?: string;
}

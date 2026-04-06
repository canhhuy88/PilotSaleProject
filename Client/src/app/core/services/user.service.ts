import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from '../../shared/models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/users`;

  getUsers(): Observable<User[]> {
    // Mock data
    return of([
      { id: '1', username: 'admin', fullName: 'Administrator', role: 'Admin' },
      { id: '2', username: 'staff1', fullName: 'Staff One', role: 'Staff' },
    ]);
  }

  createUser(user: Partial<User>): Observable<User> {
    const newUser = { ...user, id: Math.random().toString(36).substr(2, 9) } as User;
    return of(newUser);
  }

  updateUser(id: string, user: Partial<User>): Observable<User> {
    return of({ id, ...user } as User);
  }

  deleteUser(id: string): Observable<void> {
    return of(undefined);
  }
}

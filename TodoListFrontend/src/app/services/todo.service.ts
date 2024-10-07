import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TodoItem, TodoItemRequest } from '../models/todo-item.model';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  private baseUrl = 'http://localhost:5000/tasks';

  constructor(private http: HttpClient) {}

  getAll(): Observable<TodoItem[]> {
    return this.http.get<TodoItem[]>(this.baseUrl);
  }

  getById(id: number): Observable<TodoItem> {
    return this.http.get<TodoItem>(`${this.baseUrl}/${id}`);
  }

  create(todo: TodoItemRequest): Observable<TodoItem> {
    return this.http.post<TodoItem>(this.baseUrl, todo);
  }

  update(id: number, todo: TodoItemRequest): Observable<TodoItem> {
    return this.http.put<TodoItem>(`${this.baseUrl}/${id}`, todo);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  toggleCompletion(id: number): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}`, { id });
  }
}

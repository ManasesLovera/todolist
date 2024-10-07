import { Component, OnInit } from '@angular/core';
import { TodoService } from '../../services/todo.service';
import { TodoItem } from '../../models/todo-item.model';

@Component({
  selector: 'app-todo-list',
  templateUrl: './todo-list.component.html',
})
export class TodoListComponent implements OnInit {
  todos: TodoItem[] = [];
  
  constructor(private todoService: TodoService) {}

  ngOnInit(): void {
    this.fetchTodos();
  }

  fetchTodos(): void {
    this.todoService.getAll().subscribe((data: TodoItem[]) => {
      this.todos = this.sortTodos(data);
      console.log(this.todos);
    });
  }

  sortTodos(todos: TodoItem[]): TodoItem[] {
    return todos.sort((a, b) => {
      if (a.isCompleted === b.isCompleted) {
        const priorityOrder = { 'High': 1, 'Medium': 2, 'Low': 3 };
        return priorityOrder[a.priority] - priorityOrder[b.priority];
      }
      return a.isCompleted ? 1 : -1;
    });
  }

  toggleCompletion(id: number): void {
    this.todoService.toggleCompletion(id).subscribe(() => {
      this.fetchTodos();
    });
  }

  deleteTodo(id: number): void {
    this.todoService.delete(id).subscribe(() => {
      this.fetchTodos();
    });
  }
}

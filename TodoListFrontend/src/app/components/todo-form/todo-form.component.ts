import { Component, EventEmitter, Output } from '@angular/core';
import { TodoService } from '../../services/todo.service';
import { TodoItemRequest } from '../../models/todo-item.model';

@Component({
  selector: 'app-todo-form',
  templateUrl: './todo-form.component.html',
})
export class TodoFormComponent {
  todo: TodoItemRequest = { title: '', description: '', priority: 'Low', isCompleted: false };
  
  @Output() onTodoAdded = new EventEmitter<void>();

  constructor(private todoService: TodoService) {}

  createTodo(): void {
    this.todoService.create(this.todo).subscribe(() => {
      this.onTodoAdded.emit();
    });
  }
}

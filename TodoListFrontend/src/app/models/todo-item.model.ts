export interface TodoItem {
    id: number;
    title: string;
    description: string;
    priority: 'Low' | 'Medium' | 'High';
    isCompleted: boolean;
}

export interface TodoItemRequest {
    title: string;
    description: string;
    priority: 'Low' | 'Medium' | 'High';
    isCompleted: boolean;
}
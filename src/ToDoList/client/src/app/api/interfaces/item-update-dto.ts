export interface ItemUpdateDto {
  toDoItemId: number;
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate: string; 
}
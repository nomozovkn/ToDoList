export interface ItemGetDto {
  toDoItemId: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string; 
  dueDate: string;   
}
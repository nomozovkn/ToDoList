export interface IToDoFilterParamsDto {
  search?: string;
  isCompleted?: boolean;
  fromDueDate?: string; 
  toDueDate?: string;
  skip: number;
  take: number;
}
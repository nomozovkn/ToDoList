export class IToDoFilterParamsModel {
  public search?: string;
  public isCompleted?: boolean;
  public fromDueDate?: string; 
  public toDueDate?: string;
  public skip: number = 0;
  public take: number = 10;
}
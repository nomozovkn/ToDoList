export class ItemGetModel {
  public toDoItemId: number = 0;
  public title: string = ''
  public description: string = ''
  public isCompleted: boolean = false;
  public createdAt: Date = new Date(); 
  public dueDate: Date = new Date();   
}
export class ItemUpdateModel {
  public toDoItemId: number = 0;
  public title: string = '';
  public description: string = '';
  public isCompleted: boolean = false;
  public dueDate: Date = new Date();
}
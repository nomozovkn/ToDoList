import { ItemGetModel } from "./item-get-model";

export class GetAllResponseDto {
  totalCount: number = 0;
  toDoItemGetModels: ItemGetModel[] = [];
}
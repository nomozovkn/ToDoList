import { ItemGetDto } from "./item-get-dto";

export interface GetAllResponseDto {
  totalCount: number;
  toDoItemGetDtos: ItemGetDto[];
}
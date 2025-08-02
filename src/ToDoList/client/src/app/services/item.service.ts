import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, map, tap } from 'rxjs';
import { ItemApiService } from '../api/item-api-service';
import { ItemCreateModel } from './models/item-create-model';
import { ItemCreateDto } from '../api/interfaces/item-create-dto';
import { ItemGetDto } from '../api/interfaces/item-get-dto';
import { ItemGetModel } from './models/item-get-model';
import { ItemUpdateModel } from './models/item-update-model';
import { ItemUpdateDto } from '../api/interfaces/item-update-dto';

@Injectable({ providedIn: 'root' })
export class ItemService {
    router: any;
    constructor(private itemApiService: ItemApiService) { }

    public addItem(model: ItemCreateModel): Observable<number> {
        const dto = this.convertItemCreateModelToDto(model);
        return this.itemApiService.addItem(dto);
    }

    public updateItem(model: ItemUpdateModel): Observable<void> {
     
        const dto = this.convertItemUpdateModelToDto(model);
        return this.itemApiService.updateItem(dto);
    }

    public deleteItem(id: number): Observable<void> {
        return this.itemApiService.deleteItem(id);
    }

    public getAllItems(): Observable<ItemGetModel[]> {
        return this.itemApiService.getAllItems().pipe(
            map((dtoList: ItemGetDto[]) => dtoList.map(dto => this.convertItemGetDtoToModel(dto)))
        );
    }

    private convertItemCreateModelToDto(model: ItemCreateModel): ItemCreateDto {
        return {
            title: model.title,
            description: model.description,
            dueDate: model.dueDate.toString(),
        };
    }

    private convertItemUpdateModelToDto(model: ItemUpdateModel): ItemUpdateDto {
        return {
            toDoItemId: model.toDoItemId,
            title: model.title,
            description: model.description,
            isCompleted: model.isCompleted,
            dueDate: model.dueDate.toISOString(),
        };
    }

    private convertItemGetDtoToModel(dto: ItemGetDto): ItemGetModel {
        return {
            toDoItemId: dto.toDoItemId,
            title: dto.title,
            description: dto.description,
            isCompleted: dto.isCompleted,
            createdAt: new Date(dto.createdAt),
            dueDate: new Date(dto.dueDate)
        };
    }
}

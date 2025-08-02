import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ItemCreateDto } from "./interfaces/item-create-dto";
import { ItemGetDto } from "./interfaces/item-get-dto";
import { ItemUpdateDto } from "./interfaces/item-update-dto";

@Injectable({ providedIn: 'root' })
export class ItemApiService {
  private readonly apiUrl = 'https://localhost:7088/api/todo-list';

  constructor(private http: HttpClient) { }

  public addItem(dto: ItemCreateDto): Observable<number> {
    const url = `${this.apiUrl}/add`;
    return this.http.post<number>(url, dto);
  }

  public getAllItems(): Observable<ItemGetDto[]> {
    const url = `${this.apiUrl}/get-all`;
    return this.http.get<ItemGetDto[]>(url);
  }

  public updateItem(dto: ItemUpdateDto): Observable<void> {
    const url = `${this.apiUrl}/update`;
    return this.http.put<void>(url, dto);
  }

  public deleteItem(id: number): Observable<void> {
    const url = `${this.apiUrl}/delete?id=${id}`;
    return this.http.delete<void>(url);
  }
}

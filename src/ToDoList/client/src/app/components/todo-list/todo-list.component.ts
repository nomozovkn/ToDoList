import { Component, OnInit } from '@angular/core';
import { ItemService } from '../../services/item.service';
import { ItemGetModel } from '../../services/models/item-get-model';
import { ItemCreateModel } from '../../services/models/item-create-model';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ItemUpdateModel } from '../../services/models/item-update-model';
import { Console } from 'node:console';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-todo-list',
  standalone: false,
  templateUrl: './todo-list.component.html',
  styleUrl: './todo-list.component.css'
})

export class TodoListComponent implements OnInit {
  public items: ItemGetModel[] = [];
  public currentItem: ItemCreateModel | ItemUpdateModel = new ItemCreateModel();
  public isEditMode: boolean = false;
  public modalTitle: string = 'Create ToDo Item';
  public isModalOpen: boolean = false;

  constructor(private itemService: ItemService,
    private authService: AuthService,
    private router: Router,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) { }

  ngOnInit(): void {

    this.loadItems();

  }

  public loadItems(): void {

    this.spinner.show();
    this.itemService.getAllItems().subscribe({
      next: (data) => {
        this.items = data;
        this.spinner.hide(); // ✅ hide after success
      },
      error: (err) => {
        console.error(err);
        this.spinner.hide(); // ✅ hide after error too
      }
    });
  }

  public logout(): void {
    this.spinner.show();
    this.authService.logout().subscribe({
      next: () => {
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        this.spinner.hide();
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Logout failed', err);
        localStorage.removeItem('access_token');
        localStorage.removeItem('refresh_token');
        this.spinner.hide();
        this.router.navigate(['/login']);
      }
    });
  }

  public openCreateModal(): void {
    this.currentItem = new ItemCreateModel();
    this.isEditMode = false;
    this.modalTitle = 'Create ToDo Item';
    this.openModal();
  }

  public openEditModal(item: ItemGetModel): void {
    this.currentItem = {
      toDoItemId: item.toDoItemId,
      title: item.title,
      description: item.description,
      dueDate: new Date(item.dueDate),
      isCompleted: item.isCompleted
    } as ItemUpdateModel;

    this.isEditMode = true;
    this.modalTitle = 'Edit ToDo Item';
    this.openModal();
  }

  public completeItem(item: ItemGetModel): void {
    this.currentItem = {
      toDoItemId: item.toDoItemId,
      title: item.title,
      description: item.description,
      dueDate: new Date(item.dueDate),
      isCompleted: !item.isCompleted
    } as ItemUpdateModel;

    this.spinner.show();
    this.itemService.updateItem(this.currentItem as ItemUpdateModel).subscribe({
      next: () => {
        this.spinner.hide();
        this.loadItems();

      },
      error: err => {
        this.toastr.error('Failed to update item status!', 'Error');
        this.spinner.hide();
      }
    });
  }

  public saveItem(): void {
    this.spinner.show();
    if (this.isEditMode) {
      this.itemService.updateItem(this.currentItem as ItemUpdateModel).subscribe({
        next: () => {
          this.toastr.success('Data updated successfully!', 'Success');
          this.spinner.hide();
          this.loadItems();
          this.closeModal();

        },
        error: (err) => {
          this.toastr.error('Failed to update data!', 'Error');
          this.closeModal();
          this.spinner.hide();
        }
      });
    } else {
      this.itemService.addItem(this.currentItem as ItemCreateModel).subscribe({
        next: (response) => {
          this.spinner.hide();
          this.toastr.success('Data saved successfully!', 'Success');
          this.loadItems();
          this.closeModal();
        },
        error: (err) => {
          this.toastr.error('Failed to save data!', 'Error');
          this.closeModal();
          this.spinner.hide();
        }
      });
    }
  }

  public openModal(): void {
    const modal = document.getElementById('itemModal');
    this.isModalOpen = true;
  }

  public closeModal(): void {
    const modalEl = document.getElementById('itemModal');
    this.isModalOpen = false;
  }



  public showDeleteModal = false;
  public itemToDelete: ItemGetModel | null = null;

  public openDeleteModal(item: ItemGetModel): void {
    this.itemToDelete = item;
    this.showDeleteModal = true;
  }

  public cancelDelete(): void {
    this.showDeleteModal = false;
    this.itemToDelete = null;
  }

  public confirmDelete(): void {
    if (!this.itemToDelete) return;

    this.spinner.show();
    this.itemService.deleteItem(this.itemToDelete.toDoItemId).subscribe({
      next: () => {
        this.spinner.hide();
        this.toastr.success('Deleted successfully!', 'Success');
        this.loadItems();
        this.cancelDelete();
      },
      error: (err) => {
        this.spinner.hide();
        this.toastr.error('Delete failed!', 'Error');
        this.cancelDelete();
      }
    });
  }



}
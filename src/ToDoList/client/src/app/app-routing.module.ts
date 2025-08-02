import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { LoginComponent } from './components/login/login.component';
import { TodoListComponent } from './components/todo-list/todo-list.component';
import { AuthGuard } from './services/auth.guard';

const routes: Routes = [
  { path: 'sign-up', component: SignUpComponent },
  { path: 'login', component: LoginComponent },
  { path: '', component: TodoListComponent, canActivate: [AuthGuard] },
  { path: 'admin-panel', component: AdminPanelComponent, canActivate: [AuthGuard] },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { LoginModel } from '../../services/models/login-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent implements OnInit {
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  public login(): void {
    if (this.form.invalid) return;

    const model = this.form.value as LoginModel;

    this.authService.login(model).subscribe({
      next: (response) => {
        
        console.log('Login successful:', response);
        this.router.navigate(['/']); 
      },
      error: (err) => {
        console.error('Login failed:', err);
        alert('Invalid username or password.');
      }
    });
  }
}
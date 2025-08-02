import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { SignUpModel } from '../../services/models/sign-up-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  standalone: false,
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})


export class SignUpComponent implements OnInit {
  form!: FormGroup;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router ) { }

  ngOnInit() {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      phoneNumber: [''],
    });
  }

  public signUp() {
    if (this.form.valid) {
      const user: SignUpModel = this.form.value;
      this.authService.signUp(user).subscribe({
        next: () => {
          // ✅ After successful sign-up
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error('Sign-up failed:', err);
        }
      });
    }
  }
}

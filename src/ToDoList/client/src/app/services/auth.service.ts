import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { EMPTY, Observable, map, tap } from 'rxjs';
import { AuthApiService } from '../api/auth-api-service';
import { SignUpDto } from '../api/interfaces/sign-up-dto';
import { SignUpModel } from './models/sign-up-model';
import { LoginModel } from './models/login-model';
import { LoginDto } from '../api/interfaces/login-dto';
import { LoginResponseDto } from '../api/interfaces/login-response-dto';
import { LoginResponseModel } from './models/login-response-model';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
    private platformId = inject(PLATFORM_ID);

    constructor(
        private authApiService: AuthApiService,
        private router: Router
    ) { }

    private isBrowser(): boolean {
        return isPlatformBrowser(this.platformId);
    }

    public signUp(model: SignUpModel): Observable<number> {
        const dto = this.convertSignUpModelToDto(model);
        return this.authApiService.signUp(dto);
    }

    public login(model: LoginModel): Observable<LoginResponseModel> {
        const dto = this.convertLoginModelToDto(model);
        return this.authApiService.login(dto).pipe(
            tap(responseDto => {
                if (this.isBrowser()) {
                    localStorage.setItem('access_token', responseDto.accessToken);
                    localStorage.setItem('refresh_token', responseDto.refreshToken);
                }
            }),
            map(responseDto => this.convertLoginResponseDtoToModel(responseDto))
        );
    }

    public logout(): Observable<any> {
        if (!this.isBrowser()) return EMPTY;
        const refreshToken = this.getRefreshToken();
        if (!refreshToken) return EMPTY;
        return this.authApiService.logout(refreshToken);
    }

    public logout1(): void {
        debugger;
        if (!this.isBrowser()) return;

        const refreshToken = this.getRefreshToken();
        if (!refreshToken) return;

        this.authApiService.logout(refreshToken).subscribe({
            next: () => {
                localStorage.removeItem('access_token');
                localStorage.removeItem('refresh_token');
                this.router.navigate(['/login']);
            },
            error: (err) => {
                console.error('Logout failed:', err);

                localStorage.removeItem('access_token');
                localStorage.removeItem('refresh_token');

                this.router.navigate(['/login']);
            }
        });
    }

    public getAccessToken(): string | null {
        if (this.isBrowser()) {
            return localStorage.getItem('access_token');
        }
        return null;
    }

    public getRefreshToken(): string | null {
        if (this.isBrowser()) {
            return localStorage.getItem('refresh_token');
        }
        return null;
    }

    public refreshToken(): Observable<LoginResponseModel> | undefined {
        const refreshToken = this.getRefreshToken();
        const accessToken = this.getAccessToken();
        if (!refreshToken || !accessToken) return;

        return this.authApiService.refreshToken(accessToken, refreshToken).pipe(
            tap(res => {
                if (this.isBrowser()) {
                    localStorage.setItem('access_token', res.accessToken);
                    localStorage.setItem('refresh_token', res.refreshToken);
                }
            })
        );
    }

    public isLoggedIn(): boolean {
        return !!this.getAccessToken();
    }

    public testApi(): Observable<any> {
        return this.authApiService.testGetAll();
    }

    // 🔐 DTO Converters

    private convertSignUpModelToDto(model: SignUpModel): SignUpDto {
        return {
            firstName: model.firstName,
            lastName: model.lastName,
            userName: model.userName,
            email: model.email,
            password: model.password,
            phoneNumber: model.phoneNumber
        };
    }

    private convertLoginModelToDto(model: LoginModel): LoginDto {
        return {
            userName: model.userName,
            password: model.password
        };
    }

    private convertLoginResponseDtoToModel(dto: LoginResponseDto): LoginResponseModel {
        return {
            accessToken: dto.accessToken,
            refreshToken: dto.refreshToken,
            tokenType: dto.tokenType,
            expires: dto.expires
        };
    }
}

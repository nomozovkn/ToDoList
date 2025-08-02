export interface LoginResponseDto {
  accessToken: string;
  refreshToken: string;
  tokenType: string;
  expires: number;
}
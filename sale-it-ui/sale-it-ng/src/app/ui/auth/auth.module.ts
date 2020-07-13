import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './pages/login/login.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';



@NgModule({
  declarations: [LoginComponent, LoginPageComponent],
  imports: [
    CommonModule
  ]
})
export class AuthModule { }

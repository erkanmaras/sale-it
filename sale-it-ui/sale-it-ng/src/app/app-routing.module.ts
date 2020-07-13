import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginPageComponent } from './ui/auth';


const routes: Routes = [
  {
    path: '', component: LoginPageComponent
  },
  // {
  //   path: 'home',
  //   loadChildren: () => import('./ui/home/home.module').then(m => m.HomeModule)
  // },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

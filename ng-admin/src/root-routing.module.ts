import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { environment } from '@env/environment';

const routes: Routes = [
  { path: '', redirectTo: '/app/home', pathMatch: 'full' },
  {
    path: 'account',
    loadChildren: 'account/account.module#AccountModule', // Lazy load account module
    data: { preload: true },
  },
  {
    path: 'basic',
    loadChildren: 'basic-data/basic-data.module#BasicDataModule', // Lazy load account module
    data: { preload: true },
  },
];

@NgModule({
  //imports: [RouterModule.forRoot(routes)],
  imports: [RouterModule.forRoot(routes, { useHash: environment.useHash })],
  exports: [RouterModule],
  providers: [],
})
export class RootRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { VehicleManagementComponent } from './vehicle-management/vehicle-management.component';
import { DriverManagementComponent } from './driver-management/driver-management.component';
import { VehicleInformationComponent } from './vehicle-information/vehicle-information.component';
import { RouteHistoryComponent } from './route-history/route-history.component';
import { GeofencesInformationComponent } from './geofences-information/geofences-information.component';
const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'vehicle-management', component: VehicleManagementComponent },
  { path: 'drivers', component: DriverManagementComponent },
  { path: 'basicinfo', component: VehicleInformationComponent },
  { path: 'route', component: RouteHistoryComponent },
  { path: 'geofences', component: GeofencesInformationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

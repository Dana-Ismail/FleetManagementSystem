import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { HttpClientModule } from '@angular/common/http';
import { VehicleDetailComponent } from './vehicle-detail/vehicle-detail.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { VehicleManagementComponent } from './vehicle-management/vehicle-management.component';
import { DriverManagementComponent } from './driver-management/driver-management.component';
import { VehicleInformationComponent } from './vehicle-information/vehicle-information.component';
import { RouteHistoryComponent } from './route-history/route-history.component';
import { GeofencesInformationComponent } from './geofences-information/geofences-information.component';
@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    VehicleDetailComponent,
    DriverManagementComponent,
    VehicleManagementComponent,
    VehicleInformationComponent,
    RouteHistoryComponent,
    GeofencesInformationComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MatCardModule,
    RouterModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

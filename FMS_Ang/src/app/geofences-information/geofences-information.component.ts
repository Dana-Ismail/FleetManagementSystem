import { Component } from '@angular/core';
import { GeofenceService } from '../services/geofence.service';

@Component({
  selector: 'app-geofences-information',
  templateUrl: './geofences-information.component.html',
  styleUrl: './geofences-information.component.css'
})
export class GeofencesInformationComponent {
  geofences: any[] = [];

  constructor(private geofenceService: GeofenceService) { }

  ngOnInit(): void {
    this.loadGeofences();
  }

  loadGeofences(): void {
    this.geofenceService.getGeofenceInformation().subscribe(
      (response: any) => {
        if (response && response.DicOfDT && response.DicOfDT['Geofences']) {
          this.geofences = response.DicOfDT['Geofences'];
        } else {
          console.error('Geofences not found in response');
        }
      },
      error => {
        console.error('Failed to fetch geofence information:', error);
      }
    );
  }
}

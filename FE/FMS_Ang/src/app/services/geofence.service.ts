import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeofenceService {
  private apiUrl = 'https://localhost:7043/api/geofence'; 

  constructor(private http: HttpClient) { }

  getGeofenceInformation(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/all`);
  }
}

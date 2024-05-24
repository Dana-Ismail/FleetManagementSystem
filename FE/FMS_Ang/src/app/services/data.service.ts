import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { GVAR } from '../GVAR';
@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private apiUrl = 'https://localhost:7043/api/Vehicles';
  constructor(private http: HttpClient) { }
  // vehicles
  getVehicles(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/all`);
  }
  addVehicle(vehicle: GVAR): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, vehicle);
  }
  updateVehicle(vehicle: GVAR): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, vehicle);
  }
  deleteVehicle(vehicle: GVAR): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/delete`, vehicle);
  }
  // vehicle info table
  getBasicVehiclesInfo(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/basicInfo/all`);
  }
  addBasicInfo(vehicle: GVAR): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/vehiclesinformation/add`, vehicle);
  }
  updateBasicInfo(vehicle: GVAR): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/vehiclesinformation/update`, vehicle);
  }
  deleteBasicInfo(vehicle: GVAR): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/vehiclesinformation/delete`, vehicle);
  }
  // vehicle info + route 
  getAllVehicles(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/vehiclesinformation/all`);
  }
  getDetailedVehicleInformation(vehicle: GVAR): Observable<any> {
    const url = `${this.apiUrl}/vehiclesinformation/details`;
    let params = new HttpParams();
    params = params.append('DicOfDic[Tags][VehicleID]', vehicle.DicOfDic.Tags.VehicleID);
    return this.http.get<GVAR>(url, { params: params });
  }
  
  
 }

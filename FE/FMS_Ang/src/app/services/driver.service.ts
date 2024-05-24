import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { GVAR } from '../GVAR';

@Injectable({
  providedIn: 'root'
})
export class DriverService {
  private apiUrl = 'https://localhost:7043/api/Driver';
  constructor(private http: HttpClient) { }

  getDrivers(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/all`);
  }
  addDriver(driver: GVAR): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, driver);
  }
  updateDriver(driver: GVAR): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/update`, driver);
  }
}

//}




//@Injectable({
//  providedIn: 'root'
//})
//export class DriverService {
//  private apiUrl = 'https://localhost:7043/api/Drivers';

//  constructor(private http: HttpClient) { }

//  getDrivers(): Observable<any> {
//   return this.http.get<any>(`${this.apiUrl}/all`);
//   }
//  addDriver(driverName: string, phoneNumber: string): Observable<GVAR> {
//    const requestData: GVAR = {
//      DicOfDic: { Tags: { DriverName: driverName, PhoneNumber: phoneNumber } },
//      DicOfDT: {}
//    };
//    return this.http.post<GVAR>(`${this.apiUrl}/add`, requestData);
//  }

//  updateDriver(driverId: string, driverName: string, phoneNumber: string): Observable<GVAR> {
//    const requestData: GVAR = {
//      DicOfDic: { Tags: { DriverId: driverId, DriverName: driverName, PhoneNumber: phoneNumber } },
//      DicOfDT: {}
//    };
//    return this.http.post<GVAR>(`${this.apiUrl}/update`, requestData);
//  }

//  deleteDriver(driverId: string): Observable<GVAR> {
//    const requestData: GVAR = {
//      DicOfDic: { Tags: { DriverId: driverId } },
//      DicOfDT: {}
//    };
//    return this.http.post<GVAR>(`${this.apiUrl}/delete`, requestData);
//  }
//}

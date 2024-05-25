import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { GVAR } from '../GVAR';
@Injectable({
  providedIn: 'root'
})
export class RouteHistoryService {
  private apiUrl = 'https://localhost:7043/api/routeHistory';
  constructor(private http: HttpClient) { }
  addRouteHistory(requestData: GVAR): Observable<GVAR> {
    return this.http.post<any>(`${this.apiUrl}/add`, requestData);
  }

  getRouteHistory(requestData: GVAR): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/history`, requestData);
  }

}

import { Component, OnInit } from '@angular/core';
import { RouteHistoryService } from '../services/route-history.service';
import { VehicleService } from '../services/data.service';
import { GVAR } from '../GVAR';

@Component({
  selector: 'app-route-history',
  templateUrl: './route-history.component.html',
  styleUrls: ['./route-history.component.css']
})
export class RouteHistoryComponent implements OnInit {
  vehicleIDs: any[] = [];
  selectedVehicleID: number = 0;
  epochFrom: number = 0;
  epochTo: number = 0;
  showAddRouteFormFlag: boolean = false;
  showRouteHistoryFlag: boolean = false;
  routeHistory: any[] = [];
  newRouteData: any = {
    vehicleDirection: 0,
    status: '',
    vehicleSpeed: '',
    epoch: 0,
    address: '',
    latitude: 0,
    longitude: 0
  };

  constructor(
    private routeHistoryService: RouteHistoryService,
    private vehicleService: VehicleService
  ) { }

  ngOnInit(): void {
    this.loadVehicleIDs();
  }

  loadVehicleIDs(): void {
    this.vehicleService.getVehicles().subscribe(
      (response: any) => {
        if (response && response.DicOfDT && response.DicOfDT['Vehicles']) {
          this.vehicleIDs = response.DicOfDT['Vehicles'].map((vehicle: any) => ({
            VehicleID: vehicle.VehicleID,
            VehicleNumber: vehicle.VehicleNumber
          }));
        } else {
          console.error('Vehicles data not found in response');
        }
      },
      (error) => {
        console.error('Failed to fetch vehicles data:', error);
      }
    );
  }

  addRouteHistory(): void {
    const requestData = new GVAR();
    requestData.DicOfDic = {
      Tags: {
        VehicleID: this.selectedVehicleID.toString(),
        VehicleDirection: this.newRouteData.vehicleDirection.toString(),
        Status: this.newRouteData.status,
        VehicleSpeed: this.newRouteData.vehicleSpeed,
        Epoch: this.newRouteData.epoch.toString(),
        Address: this.newRouteData.address,
        Latitude: this.newRouteData.latitude.toString(),
        Longitude: this.newRouteData.longitude.toString()
      }
    };

    this.routeHistoryService.addRouteHistory(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.DicOfDic['Tags']['STS'] === '1') {
          this.showAddRouteFormFlag = false;
          console.log('Route history added successfully');
        }
      },
      (error) => {
        console.error('Failed to add route history:', error);
      }
    );
  }

  showRouteHistory(): void {
    if (this.selectedVehicleID && this.epochFrom !== 0 && this.epochTo !== 0) {
      const requestData = new GVAR();
      requestData.DicOfDic = {
        Tags: {
          VehicleID: this.selectedVehicleID.toString(),
          StartTimeEpoch: this.epochFrom.toString(),
          EndTimeEpoch: this.epochTo.toString()
        }
      };
      console.log(requestData);
      this.routeHistoryService.getRouteHistory(requestData).subscribe(
        (response: any) => {
          if (response && response.DicOfDT && response.DicOfDT['RouteHistory']) {
            this.routeHistory = response.DicOfDT['RouteHistory'];
            this.showRouteHistoryFlag = true;
          } else {
            console.error('Route history data not found in response');
            this.routeHistory = [];
          }
        },
        (error) => {
          console.error('Failed to retrieve route history:', error);
          this.routeHistory = [];
        }
      );
    } else {
      console.error('Invalid input data');
      this.routeHistory = [];
    }
  }

}

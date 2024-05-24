import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/data.service';
import { WebSocketService } from '../services/websocket.service';
import { GVAR } from '../GVAR';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  vehicles: any[] = [];
  detailedVehicle: any[] = [];
  showModal: boolean = false;

  constructor(
    private vehicleService: VehicleService,
    private webSocketService: WebSocketService
  ) { }

  ngOnInit(): void {
    this.loadVehicles();

    this.webSocketService.messages$.subscribe(
      (data: any) => {
        if (data) {
          console.log('Received updated vehicle data via WebSocket');
          this.loadVehicles();
        }
      },
      (error) => {
        console.error('Failed to receive WebSocket message:', error);
      }
    );
  }

  loadVehicles(): void {
    this.vehicleService.getAllVehicles().subscribe(
      (gvar: GVAR) => {
        if (gvar && gvar.DicOfDT && gvar.DicOfDT['VehicleInformation']) {
          this.vehicles = gvar.DicOfDT['VehicleInformation'];
        } else {
          console.error('VehicleInformation not found in GVAR response');
        }
      },
      (error) => {
        console.error('Failed to fetch vehicles data:', error);
      }
    );
  }

  showMore(vehicleID: number): void {
    const strID = vehicleID.toString();
    const requestData = new GVAR();
    if (!requestData.DicOfDic) {
      requestData.DicOfDic = {};
    }
    if (!requestData.DicOfDic.Tags) {
      requestData.DicOfDic.Tags = {};
    }
    requestData.DicOfDic.Tags.VehicleID = strID;
    this.vehicleService.getDetailedVehicleInformation(requestData).subscribe(
      (gvar: GVAR) => {
        if (gvar && gvar.DicOfDT && gvar.DicOfDT['DetailedVehicleInformation']) {
          this.detailedVehicle = gvar.DicOfDT['DetailedVehicleInformation'];
          this.showModal = true; 
          console.log('Detailed information for vehicle:', gvar.DicOfDT['DetailedVehicleInformation']);
        } else {
          console.error('DetailedVehicleInformation not found in GVAR response');
        }
      },
      (error) => {
        console.error('Failed to fetch detailed vehicle information:', error);
      }
    );
  }

  closeModal(): void {
    this.showModal = false;
  }
}

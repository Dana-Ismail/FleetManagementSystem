import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/data.service';
import { GVAR } from '../GVAR';

@Component({
  selector: 'app-vehicle-management',
  templateUrl: './vehicle-management.component.html',
  styleUrls: ['./vehicle-management.component.css']
})
export class VehicleManagementComponent implements OnInit {
  vehicles: any[] = [];
  showAddFormFlag: boolean = false;
  showUpdateFormFlag: boolean = false;
  selectedVehicle: any = {};
  newVehicleType: string = '';
  newVehicleNumber: string = '';
  showModal: boolean = false;
  detailedVehicle: any = {}; 

  constructor(private vehicleService: VehicleService) { }

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehicles().subscribe((response: any) => {
      if (response && response.DicOfDT && response.DicOfDT['Vehicles']) {
        this.vehicles = response.DicOfDT['Vehicles'];
      } else {
        console.error('Vehicles not found in response');
      }
    }, error => {
      console.error('Failed to fetch vehicles data:', error);
    });
  }

  showAddForm(): void {
    this.showAddFormFlag = true;
    this.showUpdateFormFlag = false;
  }

  showUpdateForm(vehicle: any): void {
    this.selectedVehicle = { ...vehicle };
    this.showUpdateFormFlag = true;
    this.showAddFormFlag = false;
  }

  addVehicle(): void {
    const requestData = new GVAR();
    requestData.DicOfDic = { Tags: { VehicleType: this.newVehicleType, VehicleNumber: this.newVehicleNumber } };
    this.vehicleService.addVehicle(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.dicOfDic['Tags']['STS'] === '1') {
          this.loadVehicles();
          this.showAddFormFlag = false;
          console.log('Vehicle added successfully');
          
        }
      },
      error => {
        console.error('Failed to add vehicle:', error);
      }
    );
  }

  updateVehicle(vehicleID: number): void {
    const requestData = new GVAR();
    requestData.DicOfDic = { Tags: { VehicleID: vehicleID.toString(), VehicleType: this.selectedVehicle.VehicleType, VehicleNumber: this.selectedVehicle.VehicleNumber.toString() } };
    this.vehicleService.updateVehicle(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.dicOfDic['Tags']['STS'] === '1') {
          this.loadVehicles();
          this.showUpdateFormFlag = false;
          console.log('Vehicle updated successfully');
          
        }
      },
      error => {
        console.error('Failed to update vehicle:', error);
      }
    );
  }

  deleteVehicle(vehicleID: number): void {
    const requestData = new GVAR();
    requestData.DicOfDic = { Tags: { VehicleID: vehicleID.toString() } };
    this.vehicleService.deleteVehicle(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        if (responseObject.dicOfDic['Tags']['STS'] === '1') {
          this.loadVehicles();
          this.showAddFormFlag = false;
          console.log('Vehicle deleted successfully');
        }
      },
      error => {
        console.error('Failed to delete vehicle:', error);
      }
    );
  }

}

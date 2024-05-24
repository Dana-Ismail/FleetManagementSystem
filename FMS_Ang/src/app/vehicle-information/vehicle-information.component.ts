import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../services/data.service';
import { DriverService } from '../services/driver.service';
import { GVAR } from '../GVAR';

@Component({
  selector: 'app-vehicle-information',
  templateUrl: './vehicle-information.component.html',
  styleUrls: ['./vehicle-information.component.css']
})
export class VehicleInformationComponent implements OnInit {
  vehicles: any[] = [];
  vehicleIDs: any[] = [];
  driverIDs: any[] = [];
  showAddFormFlag: boolean = false;
  showUpdateFormFlag: boolean = false;
  vehicleMake: string = '';
  vehicleModel: string = '';
  purchaseDate: number = 0;
  selectedVehicleID: number = 0;
  selectedDriverID: number = 0;
  selectedVehicle: any = {};

  constructor(private vehicleService: VehicleService, private driverService: DriverService) { }

  ngOnInit(): void {
    this.loadVehicles();
    this.loadVehicleIDs();
    this.loadDriverIDs();
  }

  loadVehicles(): void {
    this.vehicleService.getBasicVehiclesInfo().subscribe((response: any) => {
      if (response && response.DicOfDT && response.DicOfDT['VehicleInformation']) {
        this.vehicles = response.DicOfDT['VehicleInformation'];
      } else {
        console.error('Vehicles Information not found in response');
      }
    }, error => {
      console.error('Failed to fetch vehicles information data:', error);
    });
  }

  loadVehicleIDs(): void {
    this.vehicleService.getVehicles().subscribe((response: any) => {
      if (response && response.DicOfDT && response.DicOfDT['Vehicles']) {
        this.vehicleIDs = response.DicOfDT['Vehicles'].map((vehicle: any) => ({
          VehicleID: vehicle.VehicleID,
          VehicleNumber: vehicle.VehicleNumber
        }));
      } else {
        console.error('Vehicles data not found in response');
      }
    }, error => {
      console.error('Failed to fetch vehicles data:', error);
    });
  }

  loadDriverIDs(): void {
    this.driverService.getDrivers().subscribe((response: any) => {
      if (response && response.DicOfDT && response.DicOfDT['Drivers']) {
        this.driverIDs = response.DicOfDT['Drivers'].map((driver: any) => ({
          DriverID: driver.DriverID,
          DriverName: driver.DriverName
        }));
      } else {
        console.error('Drivers data not found in response');
      }
    }, error => {
      console.error('Failed to fetch drivers data:', error);
    });
  }

  showAddForm(): void {
    this.showAddFormFlag = true;
    this.showUpdateFormFlag = false;
    this.resetForm();
  }

  showUpdateForm(vehicle: any): void {
    this.selectedVehicle = { ...vehicle };
    this.selectedVehicleID = vehicle.VehicleID;
    this.selectedDriverID = vehicle.DriverID;
    this.vehicleMake = vehicle.VehicleMake;
    this.vehicleModel = vehicle.VehicleModel;
    this.purchaseDate = vehicle.PurchaseDate;
    this.showUpdateFormFlag = true;
    this.showAddFormFlag = false;
  }

  resetForm(): void {
    this.vehicleMake = '';
    this.vehicleModel = '';
    this.purchaseDate = 0;
    this.selectedVehicleID = 0;
    this.selectedDriverID = 0;
  }

  addVehicleInformation(): void {
    const requestData = new GVAR();
    requestData.DicOfDic = {
      Tags: {
        VehicleID: this.selectedVehicleID.toString(),
        DriverID: this.selectedDriverID.toString(),
        VehicleMake: this.vehicleMake,
        VehicleModel: this.vehicleModel,
        PurchaseDate: this.purchaseDate.toString()
      }
    };
    this.vehicleService.addBasicInfo(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.DicOfDic['Tags']['STS'] === '1') {
          this.loadVehicles();
          this.showAddFormFlag = false;
          console.log('Vehicle information added successfully');
        }
      },
      error => {
        console.error('Failed to add vehicle information:', error);
      }
    );
  }

  updateVehicleInformation(): void {
    const requestData = new GVAR();
    requestData.DicOfDic = {
      Tags: {
        VehicleID: this.selectedVehicle.VehicleID.toString(),
        DriverID: this.selectedDriverID.toString(),
        VehicleMake: this.vehicleMake,
        VehicleModel: this.vehicleModel,
        PurchaseDate: this.purchaseDate.toString()
      }
    };
    this.vehicleService.updateBasicInfo(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.DicOfDic['Tags']['STS'] === '1') {
          this.loadVehicles();
          this.showUpdateFormFlag = false;
          console.log('Vehicle information updated successfully');
        }
      },
      error => {
        console.error('Failed to update vehicle information:', error);
      }
    );
  }

  deleteVehicleInformation(vehicleID: number): void {
    const requestData = new GVAR();
    requestData.DicOfDic = { Tags: { VehicleID: vehicleID.toString() } };
    this.vehicleService.deleteBasicInfo(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.DicOfDic['Tags']['STS'] === '1') {
          this.loadVehicles();
          console.log('Vehicle information deleted successfully');
        }
      },
      error => {
        console.error('Failed to delete vehicle information:', error);
      }
    );
  }
}

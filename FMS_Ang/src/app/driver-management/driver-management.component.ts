import { Component, OnInit } from '@angular/core';
import { DriverService } from '../services/driver.service';
import { GVAR } from '../GVAR';

@Component({
  selector: 'app-driver-management',
  templateUrl: './driver-management.component.html',
  styleUrls: ['./driver-management.component.css']
})
export class DriverManagementComponent implements OnInit {
  drivers: any[] = [];
  showAddFormFlag: boolean = false;
  showUpdateFormFlag: boolean = false;
  selectedDriver: any = {};
  newDriverName: string = '';
  newPhoneNumber: string = '';
  showModal: boolean = false;
  constructor(private driverService: DriverService) { }

  ngOnInit(): void {
    this.loadDrivers();
  }

  loadDrivers(): void {
    this.driverService.getDrivers().subscribe((response: any) => {
      if (response && response.DicOfDT && response.DicOfDT['Drivers']) {
        this.drivers = response.DicOfDT['Drivers'];
      } else {
        console.error('Drivers not found in response');
      }
    }, error => {
      console.error('Failed to fetch drivers data:', error);
    });
  }

  showAddForm(): void {
    this.showAddFormFlag = true;
    this.showUpdateFormFlag = false;
  }

  showUpdateForm(vehicle: any): void {
    this.selectedDriver = { ...vehicle };
    this.showUpdateFormFlag = true;
    this.showAddFormFlag = false;
  }

  addDriver(): void {
    const requestData = new GVAR();
    requestData.DicOfDic = { Tags: { DriverName: this.newDriverName, PhoneNumber: this.newPhoneNumber } };
    this.driverService.addDriver(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.dicOfDic['Tags']['STS'] === '1') {
          this.loadDrivers();
          this.showAddFormFlag = false;
          console.log('Driver added successfully');
          
        }
      },
      error => {
        console.error('Failed to add driver:', error);
      }
    );
  }

  updateDriver(DriverID: number): void {
    const requestData = new GVAR();
    requestData.DicOfDic = { Tags: { DriverID: DriverID.toString(), DriverName: this.selectedDriver.DriverName, PhoneNumber: this.selectedDriver.PhoneNumber.toString() } };
    this.driverService.updateDriver(requestData).subscribe(
      (response: GVAR) => {
        const responseJson = JSON.stringify(response);
        const responseObject = JSON.parse(responseJson);
        console.log(responseJson);
        console.log(responseObject);
        if (responseObject.dicOfDic['Tags']['STS'] === '1') {
          this.loadDrivers();
          this.showUpdateFormFlag = false;
          console.log('Driver updated successfully');
         
        }
      },
      error => {
        console.error('Failed to update driver:', error);
      }
    );
  }
}

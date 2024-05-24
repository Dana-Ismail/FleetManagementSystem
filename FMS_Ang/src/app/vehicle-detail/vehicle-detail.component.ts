import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Driver } from './driver.interface';

@Component({
  selector: 'app-vehicle-detail',
  templateUrl: './vehicle-detail.component.html',
  styleUrls: ['./vehicle-detail.component.css']
})
export class VehicleDetailComponent implements OnInit {
  @Input() detailedVehicle?: any[];
  @Input() showModal?: boolean;
  @Output() close = new EventEmitter<void>();

  vehicleDetails: any = {};
  drivers: Driver[] = [];
  selectedDriver: string | undefined;
  selectedDriverPhoneNumber: string | undefined;

  constructor() { }

  ngOnInit(): void {
    if (this.detailedVehicle && this.detailedVehicle.length > 0) {
      this.vehicleDetails = this.detailedVehicle[0];
      this.extractDrivers();

      if (this.drivers.length === 1) {
        this.selectDriver(this.drivers[0].DriverName);
      } else if (this.drivers.length === 0) {
        this.selectedDriverPhoneNumber = "No drivers available";
      }
    }
  }

  extractDrivers(): void {
    console.log('Detailed vehicle data:', this.detailedVehicle);
    this.detailedVehicle?.forEach(vehicle => {
      console.log('Processing vehicle:', vehicle);
      if (vehicle) {
        const driver: Driver = {
          DriverName: vehicle.DriverName,
          PhoneNumber: vehicle.PhoneNumber,
        };
        console.log('Adding driver:', driver);
        this.drivers.push(driver);
      }
    });
    console.log('Drivers after extraction:', this.drivers);
  }

  selectDriver(driverName: string): void {
    this.selectedDriver = driverName;
    const selectedDriver = this.drivers.find(driver => driver.DriverName === driverName);
    this.selectedDriverPhoneNumber = selectedDriver ? selectedDriver.PhoneNumber : undefined;
  }

  closeModal(): void {
    this.close.emit();
  }
}

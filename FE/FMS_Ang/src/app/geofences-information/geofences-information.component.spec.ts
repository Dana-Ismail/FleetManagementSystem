import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeofencesInformationComponent } from './geofences-information.component';

describe('GeofencesInformationComponent', () => {
  let component: GeofencesInformationComponent;
  let fixture: ComponentFixture<GeofencesInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [GeofencesInformationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GeofencesInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

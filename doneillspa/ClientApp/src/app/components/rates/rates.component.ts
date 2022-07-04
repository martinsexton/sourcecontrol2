import { Component, Input } from '@angular/core';
import { LabourRate } from '../../labourrate';

import {
  ProjectService
} from '../../shared/services/project.service';

declare var $: any;

@Component({
  selector: 'rates',
  templateUrl: './rates.component.html',
  styleUrls: ['./rates.component.css']
})

export class RatesComponent {
  public labourRates: LabourRate[];
  newRate: LabourRate = new LabourRate(0, null, null, '', 0, 0);
  public selectedRole: string;
  public filteredRates: LabourRate[] = [];
  selectedRate: LabourRate = new LabourRate(0, null, null, '', 0, 0);

  constructor(private _projectService: ProjectService) {
    //Retrieve Default list of labour rates
    this._projectService.getLabourRates().subscribe(result => {
      this.labourRates = result;
      //Default to supervisor
      this.displayRatesForSelectedRole("Supervisor");
    });
  }

  displaySelectedRate(rate) {
    this.selectedRate = rate;
    $("#myDisplayRateModal").modal('show');
  }

  saveRate() {
    this._projectService.saveRate(this.newRate).subscribe(
      res => {
        this.newRate.id = res as number;
        //Update the collection of projects with newly created one
        this.labourRates.push(new LabourRate(this.newRate.id, this.newRate.effectiveFrom, this.newRate.effectiveTo, this.newRate.role, this.newRate.ratePerHour, this.newRate.overTimeRatePerHour));

        this.displayRatesForSelectedRole(this.newRate.role);

        //clear down the new project model
        this.newRate = new LabourRate(0, null, null, '', 0, 0);
        $("#myNewRateModal").modal('hide');

      });
  }

  updateRate() {
    this._projectService.updateRate(this.selectedRate).subscribe(
      res => {
        $("#myDisplayRateModal").modal('hide');
      });
  }

  deleteRate(r) {
    this._projectService.deleteRate(r).subscribe(
      res => {
        console.log(res);
        this.removeFromRatesArrays(r);
      });
  }

  removeFromRatesArrays(r: LabourRate) {
    for (let item of this.filteredRates) {
      if (item.id == r.id) {
        this.filteredRates.splice(this.filteredRates.indexOf(item), 1);
        break;
      }
    }
    for (let item of this.labourRates) {
      if (item.id == r.id) {
        this.labourRates.splice(this.filteredRates.indexOf(item), 1);
        break;
      }
    }
  }

  displayRatesForSelectedRole(role) {
    this.selectedRole = role;
    this.filteredRates = [];
    for (let r of this.labourRates) {
      if (r.role == this.selectedRole) {
        this.filteredRates.push(r);
      }
    }
  }

  retrieveRolesToDisplay() {
    let roles: string[] = [];
    roles.push("Supervisor");
    roles.push("ChargeHand");
    roles.push("ElectR1");
    roles.push("Temp");
    roles.push("First Year Apprentice");
    roles.push("Second Year Apprentice");
    roles.push("Third Year Apprentice");
    roles.push("Fourth Year Apprentice");
    roles.push("Loc1");
    roles.push("Loc2");
    roles.push("Loc3");
    roles.push("Electrical Engineer");
    roles.push("Fire Engineer");
    roles.push("General Operative");

    return roles;
  }
}

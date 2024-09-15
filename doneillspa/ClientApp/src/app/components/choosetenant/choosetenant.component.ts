import { Component, Input } from '@angular/core';
import { TenantService } from '../../shared/services/tenant.service';
import { Tenant } from '../../Tenant';
import { Router, ActivatedRoute } from '@angular/router';

declare var $: any;

@Component({
  selector: 'choosetenant',
  templateUrl: './choosetenant.component.html',
  styleUrls: ['./choosetenant.component.css']
})

export class ChooseTenantComponent {
  public tenants: Tenant[];
  public errors: string;

  constructor(private _tenantService: TenantService, private router: Router) {
    this._tenantService.getTenants().subscribe(result => {
      this.tenants = result;
      if (this.tenants.length > 0) {
        localStorage.setItem('tenant', this.tenants[0].id.toString());
        localStorage.setItem('tenantname', this.tenants[0].name);
      }
    }, error => this.errors = error);
  }

  onSelected(value: string): void {
    localStorage.setItem('tenant', value);
    for (let i = 0; i < this.tenants.length; i++) {
      let t = this.tenants[i];
      if (t.id.toString() === value) {
        localStorage.setItem('tenantname', t.name);
      }

    }
    this.router.navigate(['/dashboard']);
  }
}

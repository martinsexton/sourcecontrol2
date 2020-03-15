import { Component, Input } from '@angular/core';

import { Certificate } from '../../certificate';
import { ApplicationUser } from '../../applicationuser';
import { CertificateService } from '../../shared/services/certificate.service';
import { MsUserService } from '../../shared/services/msuser.service';

declare var $: any;

@Component({
  selector: 'user-certificates',
  templateUrl: './certification.component.html',
  styleUrls: ['./certification.component.css']
})

export class CertificationComponent {
  @Input() user: ApplicationUser;
  displayAddCert = false;
  public newCertificate: Certificate = new Certificate(0, new Date(), new Date(), "");

  constructor(private _certificationService: CertificateService, private _msuserService: MsUserService) {}

  deleteCertification(crt) {
    this._certificationService.deleteCertification(crt).subscribe(
      res => {
        this.removeFromArrayList(this.user.certifications, crt);
      });
  }
  hasCertExpired(cert: Certificate): boolean {
    var todaysDate: Date = new Date();
    var certExpiryDate: Date = new Date(cert.expiry);

    var td = new Date();
    td.setMonth(todaysDate.getMonth());
    td.setDate(todaysDate.getDate());
    td.setFullYear(todaysDate.getFullYear());

    var cd = new Date();
    cd.setMonth(certExpiryDate.getMonth());
    cd.setDate(certExpiryDate.getDate());
    cd.setFullYear(certExpiryDate.getFullYear());

    return (cd < td);
  }

  getCertificatesToDisplay() {
    return this.user.certifications;
  }

  isUserEnabled() {
    return this.user.isEnabled;
  }

  addCertificateEntry() {
    this._msuserService.addCertificate(this.user.id, this.newCertificate).subscribe(result => {
      $("#myNewCertificateModal").modal('hide');
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.newCertificate.id = result as number;
      if (!this.user.certifications) {
        this.user.certifications = [];
      }
      this.user.certifications.push(this.newCertificate)
      this.newCertificate = new Certificate(0, new Date(), new Date(), "")
    }, error => console.error(error));
  }

  removeFromArrayList(list: Certificate[], crt: Certificate) {
    for (let item of list) {
      if (crt.id == item.id) {
        list.splice(list.indexOf(item), 1);
        break;
      }
    }
  }

  toggleDisplayAddCertificate() {
    this.displayAddCert = !this.displayAddCert;
    if (this.displayAddCert) {
      $("#myNewCertificateModal").modal('show');
    } else {
      $("#myNewCertificateModal").modal('hide');
    }
  }
}

import { Component, Input } from '@angular/core';
import { ApplicationUser } from '../applicationuser';
import { MsUserService } from '../shared/services/msuser.service';
import { EmailNotification } from '../emailnotification';
import { NotificationService } from '../shared/services/notification.service';

declare var $: any;

@Component({
  selector: 'user-notifications',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})

export class NotificationComponent {
  @Input() user: ApplicationUser;
  public newEmailNotification: EmailNotification = new EmailNotification(0, '', '', '', new Date());
  displayAddNotification = false;

  constructor(private _notificationService: NotificationService, private _msuserService: MsUserService) { }

  deleteNotification(not) {
    this._notificationService.deleteNotification(not).subscribe(
      res => {
        console.log(res);
        this.removeFromNotificationArrayList(not);
      });
  }

  isUserEnabled() {
    return this.user.isEnabled;
  }

  removeFromNotificationArrayList(not: EmailNotification) {
    for (let item of this.user.emailNotifications) {
      if (not.id == item.id) {
        this.user.emailNotifications.splice(this.user.emailNotifications.indexOf(item), 1);
        break;
      }
    }
  }

  toggleDisplayAddEmailNotification() {
    this.displayAddNotification = !this.displayAddNotification;
    if (this.displayAddNotification) {
      $("#myNewEmailNotificationModal").modal('show');
    } else {
      $("#myNewEmailNotificationModal").modal('hide');
    }
  }

  addNotification() {
    this._msuserService.addEmailNotification(this.user.id, this.newEmailNotification).subscribe(result => {
      $("#myNewEmailNotificationModal").modal('hide');
      //Update the identifier of the newly created cert so if we delete it, it will be deleted on database
      this.newEmailNotification.id = result as number;

      if (!this.user.emailNotifications) {
        this.user.emailNotifications = [];
      }
      this.user.emailNotifications.push(this.newEmailNotification);

      this.newEmailNotification = new EmailNotification(0, '', '', '', new Date());
    });
  }
}

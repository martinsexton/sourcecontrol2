import { EmailNotification } from './emailnotification';

export class ApplicationUser {
  public emailNotifications: Array<EmailNotification> = new Array();

  constructor(
    public id: string,
    public firstName: string,
    public surname: string,
    public email: string,
    public phoneNumber: string,
    public role: string,
    public isEnabled: boolean,
    public tenantId : number
  ) { }
}

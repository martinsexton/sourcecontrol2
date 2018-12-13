import { Certificate } from './certificate';
import { EmailNotification } from './emailnotification';

export class ApplicationUser {
  public certifications: Array<Certificate> = new Array();
  public emailNotifications: Array<EmailNotification> = new Array();

  constructor(
    public id: string,
    public firstName: string,
    public surname: string,
    public email: string,
    public phoneNumber: string,
    public role: string
  ) { }
}

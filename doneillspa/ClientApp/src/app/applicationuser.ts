import { Certificate } from './certificate';
import { EmailNotification } from './emailnotification';
import { HolidayRequest } from './holidayrequest';

export class ApplicationUser {
  public certifications: Array<Certificate> = new Array();
  public emailNotifications: Array<EmailNotification> = new Array();
  public holidayRequests: Array<HolidayRequest> = new Array();

  constructor(
    public id: string,
    public firstName: string,
    public surname: string,
    public email: string,
    public phoneNumber: string,
    public role: string
  ) { }
}

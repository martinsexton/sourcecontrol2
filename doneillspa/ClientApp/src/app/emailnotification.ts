export class EmailNotification {
  constructor(
    public id: number,
    public body: string,
    public subject: string,
    public destination: string,
  ) { }
}

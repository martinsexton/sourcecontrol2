export class ProjectCostDto {
  public weeks: Array<string> = new Array();
  public costs: Array<number> = new Array();

  constructor(
    public projectName: string
  ) { }

}

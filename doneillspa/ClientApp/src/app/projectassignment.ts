import { UserAssignmentDetails } from "./userassignmentdetails";

export class ProjectAssignment {
  public users: Array<UserAssignmentDetails> = new Array();

  constructor(
    public code:string
  ) { }
}

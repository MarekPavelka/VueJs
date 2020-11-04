import Position from "@/models/Position";
import PositionHistory from "@/models/PositionHistory";

export default class Employee {
  Id: number = 0;
  FirstName: string = "";
  Surname: string = "";
  Address: string = "";
  Birthday: Date | null = null;
  StartDate: Date = new Date();
  Salary: number = 0;
  IsArchived: boolean = false;
  Position: Position = new Position();
  LastModified: Date | any = new Date();
  PositionHistory: PositionHistory[] = [];

  constructor(obj?: any) {
    if (!obj) return;

    this.Id = obj.Id;
    this.FirstName = obj.FirstName;
    this.Surname = obj.Surname;
    this.Address = obj.Address;
    this.Birthday = new Date(obj.Birthday);
    this.StartDate = new Date(obj.StartDate);
    this.Salary = obj.Salary;
    this.IsArchived = obj.IsArchived;
    this.Position = new Position(obj.Position);
    this.LastModified = new Date(obj.LastModified);
    this.PositionHistory = <PositionHistory[]>obj.PositionHistory;
  }
}

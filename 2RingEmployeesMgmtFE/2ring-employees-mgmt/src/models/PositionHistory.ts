export default class PositionHistory {
  Id: number = 0;
  StartDate: Date = new Date();
  EndDate: Date | null = null;
  LastModified: Date | any = new Date();

  constructor(obj?: any) {
    if (!obj) return;

    this.Id = obj.Id || 0;
    this.StartDate = obj.StartDate;
    this.LastModified = new Date(obj.LastModified);
  }
}

export default class Position {
  Id: number = 0;
  PositionName: string = "";
  LastModified: Date | any = new Date();

  constructor(obj?: any) {
    if (!obj) return;

    this.Id = obj.Id;
    this.PositionName = obj.PositionName;
    this.LastModified = new Date(obj.LastModified);
  }
}

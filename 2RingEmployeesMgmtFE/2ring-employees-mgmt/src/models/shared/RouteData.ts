export default class RouteData {
  Name: string;
  Params: Object;
  Query: Object;
  Hash: string;

  constructor(obj?: any) {
    if (!obj) return;
    this.Name = obj.name;
    this.Params = obj.params;
    this.Query = obj.query;
    this.Hash = obj.Hash;
  }
}
